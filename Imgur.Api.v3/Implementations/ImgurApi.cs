using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Imgur.Api.v3.Json;
using Imgur.Api.v3.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    public class ImgurApi : IImgurApi
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Func<Uri, Task<string>> _authorizer;
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
        {
            ContractResolver = new UnderscoreMappingResolver()
        });

        private Token _token;
        private Task<Token> _refreshTask;

        private readonly RateLimit _rateLimit = new RateLimit
        {
            ClientLimit = int.MaxValue,
            ClientRemaining = int.MaxValue,
            UserLimit = int.MaxValue,
            UserRemaining = int.MaxValue
        };

        public bool IsAuthorized
        {
            get { return Token != null; }
        }

        public Token Token
        {
            get { return _token; }
            private set
            {
                _token = value;
                if (_token != null)
                {
                    _token.Timestamp = DateTime.UtcNow;
                }
                OnTokenChanged();
            }
        }

        public ImgurApi(string clientId, string clientSecret, Func<Uri, Task<string>> authorizer, Token token)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _authorizer = authorizer;
            _token = token;
        }

        public async Task<T> ExecuteAsync<T>(IRestRequest request, bool authorize)
        {
            if (authorize)
            {
                if (!IsAuthorized)
                {
                    await Authorize();
                    if (!IsAuthorized)
                    {
                        throw new OperationCanceledException();
                    }
                }
            }
            if (IsAuthorized && DateTime.UtcNow > Token.Timestamp.AddSeconds(Token.ExpiresIn))
            {
                Token = await Refresh(Token.RefreshToken, _clientId, _clientSecret);
                if (Token == null)
                {
                    throw new OperationCanceledException();
                }
            }

            if (RateLimit.ClientLimit > 0 && RateLimit.UserLimit > 0)
            {
                var client = new HttpClient();
                var requestUri = new Uri(new Uri("https://api.imgur.com/3/"), StringUtility.Format(request.Resource, request.UrlSegments));
                var httpRequestMessage = new HttpRequestMessage(request.Method.ToHttpMethod(), requestUri);
                if (request.Parameters.Any())
                {
                    if (httpRequestMessage.Method == HttpMethod.Get)
                    {
                        var query = HttpUtility.ParseQueryString(requestUri.Query);
                        foreach (var parameter in request.Parameters)
                        {
                            query.Add(parameter.Key, Convert.ToString(parameter.Value, CultureInfo.InvariantCulture));
                        }
                        var builder = new UriBuilder(requestUri);
                        builder.Query = query.ToString();
                        httpRequestMessage.RequestUri = builder.Uri;
                    }
                    else
                    {
                        var form = request.Parameters.ToDictionary(kvp => kvp.Key, kvp => Convert.ToString(kvp.Value, CultureInfo.InvariantCulture));
                        httpRequestMessage.Content = new FormUrlEncodedContent(form);
                    }
                }
                var authenticator = authorize || IsAuthorized ? (IAuthenticator)new BearerAuthenticator(Token) : new AnonymousAuthenticator(_clientId);
                authenticator.Authenticate(httpRequestMessage);
                foreach (var header in request.Headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
                var response = await client.SendAsync(httpRequestMessage).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    UpdateRateLimits(response);
                    var responseObject = await Deserialize<Basic<T>>(response.Content).ConfigureAwait(false);
                    if (responseObject.Success)
                    {
                        return responseObject.Data;
                    }
                    throw new ImgurException("Imgur API returned non success status.");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Token.ExpiresIn = 0;
                    throw new OperationCanceledException();
                }
            }
            throw new RateLimitExceededException();
        }

        public async Task Authorize()
        {
            string authorizationUrl = string.Format("https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=code", _clientId);
            try
            {
                string code = await _authorizer(new Uri(authorizationUrl)).ConfigureAwait(false);
                Debug.WriteLine("Authorize Code: {0}", code);

                var client = new HttpClient();
                var response = await client.PostAsync("https://api.imgur.com/oauth2/token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "grant_type", "authorization_code" },
                    { "code", code }
                })).ConfigureAwait(false);
                var token = await Deserialize<Token>(response.Content).ConfigureAwait(false);

                Token = await Refresh(token.RefreshToken, _clientId, _clientSecret).ConfigureAwait(false); // get account username
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public Task SignOut()
        {
            Token = null;
            return Task.FromResult<object>(null);
        }

        public event EventHandler TokenChanged;

        public IDictionary<string, object> State
        {
            get { return _state; }
        }

        public RateLimit RateLimit
        {
            get { return _rateLimit; }
        }

        protected virtual void OnTokenChanged()
        {
            var handler = TokenChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public async Task<Token> Refresh(string refreshToken, string clientId, string clientSecret)
        {
            try
            {
                return await RefreshImplementation(refreshToken, clientId, clientSecret);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Task<Token> RefreshImplementation(string refreshToken, string clientId, string clientSecret)
        {
            if (_refreshTask != null && !_refreshTask.IsCompleted)
            {
                return _refreshTask;
            }
            return _refreshTask = CreateRefreshTask(refreshToken, clientId, clientSecret);
        }

        private async Task<Token> CreateRefreshTask(string refreshToken, string clientId, string clientSecret)
        {
            var client = new HttpClient();
            var response = await client.PostAsync("https://api.imgur.com/oauth2/token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "refresh_token" }
            })).ConfigureAwait(false);
            return await Deserialize<Token>(response.Content).ConfigureAwait(false);
        }

        private void UpdateRateLimits(HttpResponseMessage response)
        {
            if (response.Headers == null || _rateLimit == null)
            {
                return;
            }
            foreach (var header in response.Headers)
            {
                if ("X-RateLimit-UserLimit".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _rateLimit.UserLimit = Convert.ToInt32(header.Value.First());
                }
                else if ("X-RateLimit-UserRemaining".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _rateLimit.UserRemaining = Convert.ToInt32(header.Value.First());
                }
                else if ("X-RateLimit-UserReset".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _rateLimit.UserReset = Convert.ToInt32(header.Value.First());
                }
                else if ("X-RateLimit-ClientLimit".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _rateLimit.ClientLimit = Convert.ToInt32(header.Value.First());
                }
                else if ("X-RateLimit-ClientRemaining".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _rateLimit.ClientRemaining = Convert.ToInt32(header.Value.First());
                }
            }
        }

        private async Task<T> Deserialize<T>(HttpContent content)
        {
            var responseString = await content.ReadAsStringAsync().ConfigureAwait(false);
            return _jsonSerializer.Deserialize<T>(new JsonTextReader(new StringReader(responseString)));
        }
    }
}