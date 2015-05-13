using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;

namespace Imgur.Api.v3.Implementations
{
    public abstract class ImgurApiBase
    {
        private readonly RateLimit _rateLimit = new RateLimit
        {
            ClientLimit = int.MaxValue,
            ClientRemaining = int.MaxValue,
            UserLimit = int.MaxValue,
            UserRemaining = int.MaxValue
        };

        public RateLimit RateLimit
        {
            get { return _rateLimit; }
        }

        [Obsolete]
        protected Task<T> ExecuteAsync<T>(IRestClient client, IRestRequest request)
        {
            return ExecuteAsync<T>(client, request, CancellationToken.None);
        }

        [Obsolete]
        protected Task<T> ExecuteAsync<T>(IRestClient client, IRestRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}