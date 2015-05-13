using System;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    internal class BearerAuthenticator : IAuthenticator
    {
        private readonly Token _token;

        public BearerAuthenticator(Token token)
        {
            _token = token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("Authorization", String.Format("Bearer {0}", _token.AccessToken));
        }
    }
}