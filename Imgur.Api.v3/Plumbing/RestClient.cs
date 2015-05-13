using System;
using System.Net.Http;
using RestSharp.Deserializers;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    internal class RestClient : IRestClient
    {
        private readonly string _baseUrl;
        private readonly HttpClientHandler _httpClientHandler;
        private readonly HttpClient _httpClient;

        public RestClient(string baseUrl = null)
        {
            _baseUrl = baseUrl;

            _httpClientHandler = new HttpClientHandler();
            _httpClient = new HttpClient(_httpClientHandler);
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