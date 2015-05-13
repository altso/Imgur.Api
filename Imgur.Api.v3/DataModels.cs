using System.Collections.Generic;

namespace Imgur.Api.v3
{
    public interface IDisplayable
    {
        string Id { get; set; }
        string Title { get; set; }
    }

    public interface IViewable : IDisplayable
    {
        int Datetime { get; set; }
        int Views { get; set; }
        long Bandwidth { get; set; }
    }

    public interface IImage : IViewable
    {
        string Type { get; set; }
        bool Animated { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Size { get; set; }
        string Link { get; set; }
        string Gifv { get; set; }
        string Mp4 { get; set; }
        string Webm { get; set; }
    }

    public interface IAlbum : IDisplayable
    {
        string Cover { get; set; }
    }

    public abstract class GalleryItem : IViewable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Datetime { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public long Bandwidth { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
        public int Score { get; set; }
        public bool IsAlbum { get; set; }
        public string Link { get; set; }
        public string Vote { get; set; }
        public bool Favorite { get; set; }
    }

    public class Item : IDisplayable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Datetime { get; set; }
        public string Deletehash { get; set; }
    }

    public class Account
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Bio { get; set; }
        public double Reputation { get; set; }
        public string Created { get; set; }
    }

    public class AccountSettings
    {
        public string Email { get; set; }
        public bool HighQuality { get; set; }
        public bool PublicImages { get; set; }
        public bool AlbumPrivacy { get; set; }
        public string ProExpiration { get; set; }
        public string[] ActiveEmails { get; set; }
        public bool MessagingEnabled { get; set; }
        public List<BlockedUser> BlockedUsers { get; set; }
    }

    public class AccountStatistics
    {
        public int TotalImages { get; set; }
        public int TotalAlbums { get; set; }
        public string DiskUsed { get; set; }
        public string BandwidthUsed { get; set; }
        public List<Image> TopImages { get; set; }
        public List<Album> TopAlbums { get; set; }
        public List<CommentItem> TopGalleryComments { get; set; }
    }

    public class Album : Item, IAlbum
    {
        public string AccountUrl { get; set; }
        public string Privacy { get; set; }
        public string Cover { get; set; }
        public int Order { get; set; }
        public string Layout { get; set; }
        public string Link { get; set; }
        public List<Image> Images { get; set; }
    }

    public class BlockedUser
    {
        public int BlockedId { get; set; }
        public string BlockedUrl { get; set; }
    }

    public class CommentItem
    {
        public string Id { get; set; }
        public string ImageId { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public bool OnAlbum { get; set; }
        public string AlbumCover { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
        public int Points { get; set; }
        public int Datetime { get; set; }
        public string ParentId { get; set; }
        public bool Deleted { get; set; }
        public List<CommentItem> Children { get; set; }
        public string Vote { get; set; }
    }

    public class GalleryAlbum : GalleryItem, IAlbum
    {
        public string Cover { get; set; }
        public List<Image> Images { get; set; }
        public int ImagesCount { get; set; }
    }

    public class GalleryImage : GalleryItem, IImage
    {
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public string Gifv { get; set; }
        public string Mp4 { get; set; }
        public string Webm { get; set; }
        public string AccountUrl { get; set; }
    }

    public class GalleryProfile
    {
        public int TitalGalleryComments { get; set; }
        public int TotalGalleryLikes { get; set; }
        public int TotalGallerySubmissions { get; set; }
        public List<Trophy> Trophies { get; set; }
    }

    public class Image : Item, IImage
    {
        public string Link { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public string Gifv { get; set; }
        public string Mp4 { get; set; }
        public string Webm { get; set; }
        public int Views { get; set; }
        public long Bandwidth { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public string From { get; set; }
        public int AccountId { get; set; }
        public int RecipientAccountId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Timestamp { get; set; }
        public int ParentId { get; set; }
    }

    public class Reply
    {
        
    }

    public class Notification<T>
    {
        public string Id { get; set; }
        public int AccountId { get; set; }
        public int Viewed { get; set; }
        public T Content { get; set; }
    }

    public class Notifications
    {
        public List<Notification<Message>> Messages { get; set; }
        public List<Notification<CommentItem>> Replies { get; set; }
    }

    public class Trophy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameClean { get; set; }
        public string Description { get; set; }
        public object Data { get; set; }
    }

    public class Vote
    {
        public int Ups { get; set; }
        public int Downs { get; set; }
    }
}