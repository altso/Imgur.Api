using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Imgur.Api.v3
{
    public interface IImgurApi
    {
        RateLimit RateLimit { get; }
        bool IsAuthorized { get; }
        Token Token { get; }
        Task<T> ExecuteAsync<T>(IRestRequest request, bool authorize);
        Task Authorize();
        Task SignOut();
        event EventHandler TokenChanged;
        IDictionary<string, object> State { get; }
    }
}