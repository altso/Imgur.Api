
using System;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    internal interface IAuthenticator
    {
        void Authenticate(IRestRequest request);
    }
}