using System;
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

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("Authorization", String.Format("Client-ID {0}", _clientId));
        }
    }
}