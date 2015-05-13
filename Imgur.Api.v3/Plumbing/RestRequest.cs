using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    internal class RestRequest : IRestRequest
    {
        public RestRequest(string url = null, Method method = Method.GET)
        {
            Method = method;
        }

        public Method Method { get; set; }

        public IRestRequest AddUrlSegment(string name, string value)
        {
            return this;
        }

        public IRestRequest AddParameter(string name, object value)
        {
            return this;
        }

        public IRestRequest AddHeader(string name, string value)
        {
            return this;
        }

        public IRestRequest AddFile(string name, Action<Stream> writer, string fileName)
        {
            return this;
        }
    }
}