
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    internal interface IAuthenticator
    {
        void Authenticate(HttpRequestMessage request);
    }
}