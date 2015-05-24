using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3
{
    public interface IExecutor
    {
        Task<T> ExecuteAsync<T>(IRestRequest request, bool authorize, IProgress<double> requestProgress = null);
        IDictionary<string, object> State { get; }
    }
}