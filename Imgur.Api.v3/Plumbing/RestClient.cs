using System;
using RestSharp.Deserializers;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    internal class RestClient : IRestClient
    {
        public RestClient(string baseUrl = null)
        {
        }

        public IAuthenticator Authenticator { get; set; }

        public void ClearHandlers()
        {
        }

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
        }
    }
}