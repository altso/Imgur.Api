using System.Net.Http;
using System.Net.Http.Headers;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    internal class BearerAuthenticator : IAuthenticator
    {
        private readonly Token _token;

        public BearerAuthenticator(Token token)
        {
            _token = token;
        }

        public void Authenticate(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);
        }
    }
}