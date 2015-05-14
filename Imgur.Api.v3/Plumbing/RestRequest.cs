using System;
using System.Collections.Generic;
using System.IO;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    internal class RestRequest : IRestRequest
    {
        private readonly Lazy<IDictionary<string, string>> _urlSegments = new Lazy<IDictionary<string, string>>(() => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
        private readonly Lazy<IDictionary<string, object>> _parameters = new Lazy<IDictionary<string, object>>(() => new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
        private readonly Lazy<IDictionary<string, string>> _headers = new Lazy<IDictionary<string, string>>(() => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

        public RestRequest(string resource = null, Method method = Method.GET)
        {
            Resource = resource;
            Method = method;
        }

        public string Resource { get; set; }

        public Method Method { get; set; }

        public IDictionary<string, string> UrlSegments
        {
            get { return _urlSegments.Value; }
        }

        public IDictionary<string, object> Parameters
        {
            get { return _parameters.Value; }
        }

        public IDictionary<string, string> Headers
        {
            get { return _headers.Value; }
        }

        public IRestRequest AddUrlSegment(string name, string value)
        {
            UrlSegments[name] = value;
            return this;
        }

        public IRestRequest AddParameter(string name, object value)
        {
            Parameters[name] = value;
            return this;
        }

        public IRestRequest AddHeader(string name, string value)
        {
            Headers[name] = value;
            return this;
        }

        public IRestRequest AddFile(string name, Action<Stream> writer, string fileName)
        {
            return this;
        }
    }
}