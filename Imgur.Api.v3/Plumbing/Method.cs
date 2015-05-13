using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    public enum Method
    {
        // ReSharper disable InconsistentNaming
        GET,
        POST,
        PUT,
        DELETE
    }

    internal static class MethodExtensions
    {
        public static HttpMethod ToHttpMethod(this Method method)
        {
            switch (method)
            {
                case Method.GET:
                    return HttpMethod.Get;
                case Method.POST:
                    return HttpMethod.Post;
                case Method.PUT:
                    return HttpMethod.Put;
                case Method.DELETE:
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }
        }
    }
}