
using System;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    internal interface IAuthenticator
    {
        void Authenticate(IRestClient client, IRestRequest request);
    }
}