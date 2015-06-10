using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IGalleryEndpoint
    {
        Task<IEnumerable<GalleryItem>> GetGallery(GallerySection section, GallerySort sort, GalleryWindow window, int page, bool showViral);
        Task<IEnumerable<GalleryItem>> GetRandom(int page);
        Task<IEnumerable<GalleryItem>> Search(string q);
        Task<IEnumerable<GalleryItem>> GetSubreddits(string subreddit, GallerySort sort, GalleryWindow window, int page);
        Task<GalleryItem> GetItem(string id);
        Task<GalleryImage> GetImage(string id);
        Task<GalleryAlbum> GetAlbum(string id);
        Task<GalleryItem> GetSubredditItem(string subreddit, string id);
        Task<IEnumerable<CommentItem>> GetComments(string id);
        Task Vote(string id, string vote);
        Task Submit(string id, string title, bool terms);
    }

    public enum GallerySection
    {
        Hot, Top, User
    }

    public enum GallerySort
    {
        Viral, Time, Top, Rising
    }

    public enum GalleryWindow
    {
        Day, Week, Month, Year, All
    }
}