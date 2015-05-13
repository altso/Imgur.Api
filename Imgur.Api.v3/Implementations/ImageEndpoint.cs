using System;
using System.IO;
using System.Threading.Tasks;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    public class ImageEndpoint : IImageEndpoint
    {
        private readonly IImgurApi _imgurApi;

        public ImageEndpoint(IImgurApi imgurApi)
        {
            _imgurApi = imgurApi;
        }

        public Task<Image> Get(string id)
        {
            return _imgurApi.ExecuteAsync<Image>(new RestRequest("image/{id}").AddUrlSegment("id", id), false);
        }

        public Task Update(string id, string title, string description)
        {
            try
            {
                return _imgurApi.ExecuteAsync<bool>(
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
                return _imgurApi.ExecuteAsync<bool>(
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
            return _imgurApi.ExecuteAsync<string>(new RestRequest("image/{id}/favorite", Method.POST).AddUrlSegment("id", id), true);
        }

        public Task<Image> Upload(Stream image, string albumId, string name, string title, string description, IProgress<double> progress)
        {
            try
            {
                var request = new RestRequest("image", Method.POST)
                    .AddFile("image", writer =>
                    {
                        image.Seek(0L, SeekOrigin.Begin);
                        var array = new byte[4096];
                        int count, totalCount = 0;
                        while ((count = image.Read(array, 0, array.Length)) != 0)
                        {
                            writer.Write(array, 0, count);
                            totalCount += count;
                            progress.Report(totalCount * 1d / image.Length);
                            writer.Flush();
                        }
                    }, name)
                    .AddParameter("album", albumId)
                    .AddParameter("type", "file")
                    .AddParameter("title", title)
                    .AddParameter("description", description);
                return _imgurApi.ExecuteAsync<Image>(request, false);
            }
            finally
            {
                IncrementVersion();
            }
        }

        private void IncrementVersion()
        {
            object version;
            if (_imgurApi.State.TryGetValue("image", out version))
            {
                _imgurApi.State["image"] = Convert.ToInt32(version) + 1;
            }
            else
            {
                _imgurApi.State["image"] = 0;
            }
        }
    }
}