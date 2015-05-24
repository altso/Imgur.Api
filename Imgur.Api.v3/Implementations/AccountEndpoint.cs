using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class AccountEndpoint : IAccountEndpoint
    {
        private readonly IExecutor _executor;

        public AccountEndpoint(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task<Account> GetAccount(string userName = "me")
        {
            var request = new RestRequest("account/{username}")
                .AddUrlSegment("username", userName);
            return await _executor.ExecuteAsync<Account>(request, true).ConfigureAwait(false);
        }

        public async Task<Account> Create(string userName, string recaptchaChallenge, string recaptchaResponse)
        {
            var request = new RestRequest("account/{username}", Method.POST)
                .AddUrlSegment("username", userName)
                .AddParameter("recaptcha_challenge_field", recaptchaChallenge)
                .AddParameter("recaptcha_response_field", recaptchaResponse);
            return await _executor.ExecuteAsync<Account>(request, false).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CommentItem>> GetComments(string userName = "me")
        {
            var request = new RestRequest("account/{username}/comments")
                .AddUrlSegment("username", userName);
            return await _executor.ExecuteAsync<List<CommentItem>>(request, true).ConfigureAwait(false);
        }

        public Task<IEnumerable<Item>> GetSubmissions(string userName = "me", int page = 0)
        {
            var request = new RestRequest("account/{username}/submissions/{page}")
                .AddUrlSegment("username", userName)
                .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture));
            return GetItems(request);
        }

        public Task<IEnumerable<Item>> GetFavorites(string userName = "me")
        {
            var request = new RestRequest("account/{username}/favorites")
                .AddUrlSegment("username", userName);
            return GetItems(request);
        }

        public Task<IEnumerable<Item>> GetGalleryFavorites(string userName = "me")
        {
            var request = new RestRequest("account/{username}/gallery_favorites")
                .AddUrlSegment("username", userName);
            return GetItems(request);
        }

        public Task<int> GetImageCount(string userName = "me")
        {
            var request = new RestRequest("account/{username}/images/count")
                .AddUrlSegment("username", userName);
            return _executor.ExecuteAsync<int>(request, true);
        }

        public Task<int> GetAlbumCount(string userName = "me")
        {
            var request = new RestRequest("account/{username}/albums/count")
                .AddUrlSegment("username", userName);
            return _executor.ExecuteAsync<int>(request, true);
        }

        public Task<int> GetCommentCount(string userName = "me")
        {
            var request = new RestRequest("account/{username}/comments/count")
                .AddUrlSegment("username", userName);
            return _executor.ExecuteAsync<int>(request, true);
        }

        public Task<GalleryProfile> GetGalleryProfile(string userName = "me")
        {
            var request = new RestRequest("account/{username}/gallery_profile")
                .AddUrlSegment("username", userName);
            return _executor.ExecuteAsync<GalleryProfile>(request, true);
        }

        public async Task<IEnumerable<Image>> GetImages(string userName = "me", int page = 0)
        {
            var request = new RestRequest("account/{username}/images/{page}")
                .AddUrlSegment("username", userName)
                .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture));

            object version;
            if (_executor.State.TryGetValue("image", out version))
            {
                request = request.AddParameter("v", version);
            }

            return await _executor.ExecuteAsync<List<Image>>(request, true).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Album>> GetAlbums(string userName = "me", int page = 0)
        {
            var request = new RestRequest("account/{username}/albums/{page}")
                .AddUrlSegment("username", userName)
                .AddUrlSegment("page", page.ToString(CultureInfo.InvariantCulture));

            object version;
            if (_executor.State.TryGetValue("album", out version))
            {
                request = request.AddParameter("v", version);
            }

            return await _executor.ExecuteAsync<List<Album>>(request, true).ConfigureAwait(false);
        }

        private async Task<IEnumerable<Item>> GetItems(IRestRequest request)
        {
            var items = await _executor.ExecuteAsync<List<AlbumOrImage>>(request, true).ConfigureAwait(false);
            return items.Select(i => i.ToItem());
        }
    }
}