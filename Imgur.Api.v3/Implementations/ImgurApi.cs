using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;

namespace Imgur.Api.v3.Implementations
{
    public class ImgurApi : ImgurApiBase, IImgurApi
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Func<Uri, Task<string>> _authorizer;
        private readonly IDictionary<string, object> _state = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private Token _token;
        private Task<Token> _refreshTask;

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
                var client = new RestClient("https://api.imgur.com/3/");
                client.ClearHandlers();
                client.AddHandler("*", new JsonDeserializer());
                client.Authenticator = authorize || IsAuthorized ? (IAuthenticator)new BearerAuthenticator(Token) : new AnonymousAuthenticator(_clientId);
                try
                {
                    var response = await ExecuteAsync<Basic<T>>(client, request).ConfigureAwait(false);
                    Debug.WriteLine(RateLimit);
                    if (response.Success)
                    {
                        return response.Data;
                    }
                }
                catch (NotOkException e)
                {
                    if (e.StatusCode == HttpStatusCode.Forbidden)
                    {
                        Token.ExpiresIn = 0;
                        throw new OperationCanceledException();
                    }
                    throw;
                }
                throw new ImgurException("Imgur API returned non success status.");
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

                var client = new RestClient("https://api.imgur.com/oauth2/token");
                var request = new RestRequest()
                    .AddParameter("client_id", _clientId)
                    .AddParameter("client_secret", _clientSecret)
                    .AddParameter("grant_type", "authorization_code")
                    .AddParameter("code", code);
                request.Method = Method.POST;
                var token = await ExecuteAsync<Token>(client, request).ConfigureAwait(false);
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

        private Task<Token> CreateRefreshTask(string refreshToken, string clientId, string clientSecret)
        {
            var client = new RestClient("https://api.imgur.com/oauth2/token");
            var request = new RestRequest()
                .AddParameter("refresh_token", refreshToken)
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", clientSecret)
                .AddParameter("grant_type", "refresh_token");
            request.Method = Method.POST;
            return ExecuteAsync<Token>(client, request);
        }
    }
}