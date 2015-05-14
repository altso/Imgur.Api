using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    internal class AnonymousAuthenticator : IAuthenticator
    {
        private readonly string _clientId;

        public AnonymousAuthenticator(string clientId)
        {
            _clientId = clientId;
        }

        public void Authenticate(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", _clientId);
        }
    }
}