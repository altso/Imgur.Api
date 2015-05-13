using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IImgurApi
    {
        RateLimit RateLimit { get; }
        bool IsAuthorized { get; }
        Token Token { get; }
        Task Authorize();
        Task SignOut();
        event EventHandler TokenChanged;
        IDictionary<string, object> State { get; }
    }
}