using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class AlbumEndpoint : IAlbumEndpoint
    {
        private readonly IExecutor _executor;

        public AlbumEndpoint(IExecutor executor)
        {
            _executor = executor;
        }

        public Task<Album> Get(string id)
        {
            return _executor.ExecuteAsync<Album>(
                new RestRequest("album/{id}")
                    .AddUrlSegment("id", id),
                false);
        }

        public Task Update(string id, IEnumerable<string> ids, string title, string description, string coverId)
        {
            try
            {
                var request = new RestRequest("album/{id}", Method.PUT)
                    .AddUrlSegment("id", id)
                    .AddParameter("title", title)
                    .AddParameter("description", description)
                    .AddParameter("cover", coverId);
                if (ids != null)
                {
                    request.AddParameter("ids", string.Join(",", ids));
                }
                return _executor.ExecuteAsync<bool>(request, false);
            }
            finally
            {
                IncrementVersion();
            }
        }

        public Task Delete(string id)
        {
            try
            {
                return _executor.ExecuteAsync<bool>(new RestRequest("album/{id}", Method.DELETE).AddUrlSegment("id", id), false);
            }
            finally
            {
                IncrementVersion();
            }
        }

        public Task<string> Favorite(string id)
        {
            return _executor.ExecuteAsync<string>(
                new RestRequest("album/{id}/favorite", Method.POST)
                    .AddUrlSegment("id", id),
                true);
        }

        public Task<Album> Create(IEnumerable<string> ids, string title, string description, string coverId)
        {
            try
            {
                return _executor.ExecuteAsync<Album>(new RestRequest("album", Method.POST)
                    .AddParameter("ids", string.Join(",", ids))
                    .AddParameter("title", title)
                    .AddParameter("description", description)
                    .AddParameter("cover", coverId), false);
            }
            finally
            {
                IncrementVersion();
            }
        }

        private void IncrementVersion()
        {
            object version;
            if (_executor.State.TryGetValue("album", out version))
            {
                _executor.State["album"] = Convert.ToInt32(version) + 1;
            }
            else
            {
                _executor.State["album"] = 0;
            }
        }
    }
}