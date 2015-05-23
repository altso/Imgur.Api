using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class AlbumEndpoint : IAlbumEndpoint
    {
        private readonly IImgurApi _imgurApi;

        public AlbumEndpoint(IImgurApi imgurApi)
        {
            _imgurApi = imgurApi;
        }

        public Task<Album> Get(string id)
        {
            return _imgurApi.ExecuteAsync<Album>(
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
                return _imgurApi.ExecuteAsync<bool>(request, false);
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
                return _imgurApi.ExecuteAsync<bool>(new RestRequest("album/{id}", Method.DELETE).AddUrlSegment("id", id), false);
            }
            finally
            {
                IncrementVersion();
            }
        }

        public Task<string> Favorite(string id)
        {
            return _imgurApi.ExecuteAsync<string>(
                new RestRequest("album/{id}/favorite", Method.POST)
                    .AddUrlSegment("id", id),
                true);
        }

        public Task<Album> Create(IEnumerable<string> ids, string title, string description, string coverId)
        {
            try
            {
                return _imgurApi.ExecuteAsync<Album>(new RestRequest("album", Method.POST)
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
            if (_imgurApi.State.TryGetValue("album", out version))
            {
                _imgurApi.State["album"] = Convert.ToInt32(version) + 1;
            }
            else
            {
                _imgurApi.State["album"] = 0;
            }
        }
    }
}