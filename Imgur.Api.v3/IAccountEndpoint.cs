using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IAccountEndpoint
    {
        Task<Account> GetAccount(string userName = "me");
        Task<Account> Create(string userName, string recaptchaChallenge, string recaptchaResponse);
        Task<IEnumerable<CommentItem>> GetComments(string userName = "me");
        Task<IEnumerable<Item>> GetSubmissions(string userName = "me", int page = 0);
        Task<IEnumerable<Item>> GetFavorites(string userName = "me");
        Task<IEnumerable<Item>> GetGalleryFavorites(string userName = "me");
        Task<int> GetImageCount(string userName = "me");
        Task<int> GetAlbumCount(string userName = "me");
        Task<int> GetCommentCount(string userName = "me");
        Task<GalleryProfile> GetGalleryProfile(string userName = "me");
        Task<IEnumerable<Image>> GetImages(string userName = "me", int page = 0);
        Task<IEnumerable<Album>> GetAlbums(string userName = "me", int page = 0);
    }
}