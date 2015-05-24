using System;
using System.IO;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class ImageEndpoint : IImageEndpoint
    {
        private readonly IExecutor _executor;

        public ImageEndpoint(IExecutor executor)
        {
            _executor = executor;
        }

        public Task<Image> Get(string id)
        {
            return _executor.ExecuteAsync<Image>(new RestRequest("image/{id}").AddUrlSegment("id", id), false);
        }

        public Task Update(string id, string title, string description)
        {
            try
            {
                return _executor.ExecuteAsync<bool>(
                    new RestRequest("image/{id}", Method.POST)
                        .AddUrlSegment("id", id)
                        .AddParameter("title", title)
                        .AddParameter("description", description),
                    false);

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
                return _executor.ExecuteAsync<bool>(
                    new RestRequest("image/{id}", Method.DELETE)
                        .AddUrlSegment("id", id),
                    false);

            }
            finally
            {
                IncrementVersion();
            }
        }

        public Task<string> Favorite(string id)
        {
            return _executor.ExecuteAsync<string>(new RestRequest("image/{id}/favorite", Method.POST).AddUrlSegment("id", id), true);
        }

        public async Task<Image> Upload(Stream image, string albumId, string name, string title, string description, IProgress<double> progress)
        {
            try
            {
                var request = new RestRequest("image", Method.POST)
                    .AddFile("image", image, name)
                    .AddParameter("album", albumId)
                    .AddParameter("type", "file")
                    .AddParameter("title", title)
                    .AddParameter("description", description);
                var response = await _executor.ExecuteAsync<Image>(request, false, progress);
                return response;
            }
            finally
            {
                IncrementVersion();
            }
        }

        private void IncrementVersion()
        {
            object version;
            if (_executor.State.TryGetValue("image", out version))
            {
                _executor.State["image"] = Convert.ToInt32(version) + 1;
            }
            else
            {
                _executor.State["image"] = 0;
            }
        }
    }
}