using System;
using System.Collections.Generic;
using System.IO;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    public interface IRestRequest
    {
        string Resource { get; set; }
        Method Method { get; set; }
        IDictionary<string, string> UrlSegments { get; }
        IDictionary<string, object> Parameters { get; }
        IDictionary<string, string> Headers { get; }
        IRestRequest AddUrlSegment(string name, string value);
        IRestRequest AddParameter(string name, object value);
        IRestRequest AddHeader(string name, string value);
        IRestRequest AddFile(string name, Action<Stream> writer, string fileName);
    }
}