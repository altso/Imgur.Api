using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    public class GalleryEndpoint : IGalleryEndpoint
    {
        private readonly IImgurApi _api;

        public GalleryEndpoint(IImgurApi api)
        {
            _api = api;
        }

        public async Task<IEnumerable<GalleryItem>> GetGallery(GallerySection section, GallerySort sort, GalleryWindow window, int page, bool showViral)
        {
            var items = await _api.ExecuteAsync<List<GalleryAlbumOrImage>>(
                new RestRequest("gallery/{section}/{sort}/{window}/{page}")
                    .AddUrlSegment("section", section.ToString().ToLowerInvariant())
                    .AddUrlSegment("sort", sort.ToString().ToLowerInvariant())
                    .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture))
                    .AddUrlSegment("window", window.ToString().ToLowerInvariant())
                    .AddParameter("showViral", showViral.ToString().ToLowerInvariant()),
                false).ConfigureAwait(false);
            return items.Select(i => i.ToGalleryItem());
        }

        public async Task<IEnumerable<GalleryItem>> GetRandom(int page)
        {
            var items = await _api.ExecuteAsync<List<GalleryAlbumOrImage>>(
                new RestRequest("gallery/random/random/{page}")
                    .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture)),
                false).ConfigureAwait(false);
            return items.Select(i => i.ToGalleryItem());
        }

        public async Task<IEnumerable<GalleryItem>> Search(string q)
        {
            var items = await _api.ExecuteAsync<List<GalleryAlbumOrImage>>(
                new RestRequest("gallery/search")
                    .AddParameter("q", q),
                false).ConfigureAwait(false);
            return items.Select(i => i.ToGalleryItem());
        }

        public async Task<IEnumerable<GalleryItem>> GetSubreddits(string subreddit, GallerySort sort, GalleryWindow window, int page)
        {
            if (!subreddit.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                subreddit = subreddit + "/";
            }
            var items = await _api.ExecuteAsync<List<GalleryAlbumOrImage>>(
                new RestRequest(string.Format("gallery{0}{{sort}}/{{window}}/{{page}}", subreddit))
                    .AddUrlSegment("sort", sort.ToString().ToLowerInvariant())
                    .AddUrlSegment("window", window.ToString().ToLowerInvariant())
                    .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture)),
                false).ConfigureAwait(false);
            return items.Select(i => i.ToGalleryItem());
        }

        public async Task<GalleryItem> GetItem(string id)
        {
            var item = await _api.ExecuteAsync<GalleryAlbumOrImage>(new RestRequest("gallery/{id}").AddUrlSegment("id", id), false).ConfigureAwait(false);
            return item.ToGalleryItem();
        }

        public async Task<GalleryImage> GetImage(string id)
        {
            var item = await _api.ExecuteAsync<GalleryImage>(new RestRequest("gallery/image/{id}").AddUrlSegment("id", id), false).ConfigureAwait(false);
            return item;
        }

        public async Task<GalleryAlbum> GetAlbum(string id)
        {
            return await _api.ExecuteAsync<GalleryAlbum>(
                new RestRequest("gallery/album/{id}")
                    .AddUrlSegment("id", id),
                false).ConfigureAwait(false);
        }

        public async Task<GalleryItem> GetSubredditItem(string subreddit, string id)
        {
            var item = await _api.ExecuteAsync<GalleryAlbumOrImage>(
                new RestRequest("gallery{subreddit}{id}")
                    .AddUrlSegment("subreddit", subreddit)
                    .AddUrlSegment("id", id), false)
                                 .ConfigureAwait(false);
            return item.ToGalleryItem();
        }

        public async Task<IEnumerable<CommentItem>> GetComments(string id)
        {
            return await _api.ExecuteAsync<List<CommentItem>>(
                new RestRequest("gallery/{id}/comments")
                    .AddUrlSegment("id", id),
                false).ConfigureAwait(false);
        }

        public async Task Vote(string id, string vote)
        {
            await _api.ExecuteAsync<bool>(
                new RestRequest("gallery/{id}/vote/{vote}", Method.POST)
                    .AddUrlSegment("id", id)
                    .AddUrlSegment("vote", vote),
                true).ConfigureAwait(false);
        }

        public async Task Submit(string id, string title, bool terms)
        {
            await _api.ExecuteAsync<bool>(
                new RestRequest("gallery/{id}", Method.POST)
                    .AddUrlSegment("id", id)
                    .AddParameter("title", title)
                    .AddParameter("terms", terms ? 1 : 0),
                true).ConfigureAwait(false);
        }
    }
}