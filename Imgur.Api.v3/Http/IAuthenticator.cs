using System.Net.Http;

namespace Imgur.Api.v3.Http
{
    internal interface IAuthenticator
    {
        void Authenticate(HttpRequestMessage request);
    }
}