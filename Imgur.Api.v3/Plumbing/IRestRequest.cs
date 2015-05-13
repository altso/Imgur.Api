using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    [Obsolete]
    public interface IRestRequest
    {
        Method Method { get; set; }
        IRestRequest AddUrlSegment(string name, string value);
        IRestRequest AddParameter(string name, object value);
        IRestRequest AddHeader(string name, string value);
        IRestRequest AddFile(string name, Action<Stream> writer, string fileName);
    }
}