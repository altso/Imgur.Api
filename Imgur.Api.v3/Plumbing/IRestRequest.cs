using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using Imgur.Api.v3.Utilities;

// ReSharper disable once CheckNamespace
namespace RestSharp
{
    public interface IRestRequest
    {
        string Resource { get; set; }
        Method Method { get; set; }
        IDictionary<string, string> UrlSegments { get; }
        IDictionary<string, object> Parameters { get; }
        IDictionary<string, string> Headers { get; }
        IDictionary<string, File> Files { get; }
        IRestRequest AddUrlSegment(string name, string value);
        IRestRequest AddParameter(string name, object value);
        IRestRequest AddHeader(string name, string value);
        IRestRequest AddFile(string name, Stream stream, string fileName);
    }

    public class File
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }

    public static class RestRequestExtensions
    {
        public static HttpRequestMessage ToHttpRequestMessage(this IRestRequest request, Uri baseUri)
        {
            var requestUri = new Uri(baseUri, StringUtility.Format(request.Resource, request.UrlSegments));
            var httpRequestMessage = new HttpRequestMessage(request.Method.ToHttpMethod(), requestUri);
            if (request.Files.Any())
            {
                var formDataContent = new MultipartFormDataContent();
                foreach (var file in request.Files)
                {
                    formDataContent.Add(new StreamContent(file.Value.Stream), file.Key, file.Value.FileName);
                }
                foreach (var parameter in request.Parameters)
                {
                    formDataContent.Add(new StringContent(Convert.ToString(parameter.Value, CultureInfo.InvariantCulture)), parameter.Key);
                }

                httpRequestMessage.Content = formDataContent;
            }
            else if (request.Parameters.Any())
            {
                if (httpRequestMessage.Method == HttpMethod.Get)
                {
                    var query = requestUri.ParseQueryString();
                    foreach (var parameter in request.Parameters)
                    {
                        query.Add(parameter.Key, Convert.ToString(parameter.Value, CultureInfo.InvariantCulture));
                    }
                    var builder = new UriBuilder(requestUri) { Query = query.ToString() };
                    httpRequestMessage.RequestUri = builder.Uri;
                }
                else
                {
                    var form = request.Parameters.ToDictionary(kvp => kvp.Key, kvp => Convert.ToString(kvp.Value, CultureInfo.InvariantCulture));
                    httpRequestMessage.Content = new FormUrlEncodedContent(form);
                }
            }
            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
            return httpRequestMessage;
        }

    }
}