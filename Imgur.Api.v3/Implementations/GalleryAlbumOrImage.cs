using System.Collections.Generic;

namespace Imgur.Api.v3.Implementations
{
    public class GalleryAlbumOrImage : GalleryItem, IAlbum, IImage
    {
        public List<Image> Images { get; set; }
        public int ImagesCount { get; set; }
        public string Cover { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public string Gifv { get; set; }
        public string Mp4 { get; set; }
        public string Webm { get; set; }

        public GalleryItem ToGalleryItem()
        {
            if (IsAlbum)
            {
                return ToGalleryAlbum();
            }
            return ToGalleryImage();
        }

        public GalleryImage ToGalleryImage()
        {
            return new GalleryImage
            {
                AccountUrl = AccountUrl,
                Animated = Animated,
                Bandwidth = Bandwidth,
                CommentCount = CommentCount,
                CommentPreview = CommentPreview,
                Datetime = Datetime,
                Description = Description,
                Downs = Downs,
                Height = Height,
                Id = Id,
                IsAlbum = IsAlbum,
                Score = Score,
                Size = Size,
                Title = Title,
                Type = Type,
                Ups = Ups,
                Views = Views,
                Width = Width,
                Link = Link,
                Vote = Vote,
                Favorite = Favorite,
                Gifv = Gifv,
                Mp4 = Mp4,
                Webm = Webm
            };
        }

        public GalleryAlbum ToGalleryAlbum()
        {
            return new GalleryAlbum
            {
                AccountUrl = AccountUrl,
                Images = Images,
                ImagesCount = ImagesCount,
                CommentCount = CommentCount,
                CommentPreview = CommentPreview,
                Bandwidth = Bandwidth,
                Cover = Cover,
                Datetime = Datetime,
                Description = Description,
                Downs = Downs,
                Id = Id,
                IsAlbum = IsAlbum,
                Score = Score,
                Title = Title,
                Ups = Ups,
                Views = Views,
                Link = Link,
                Vote = Vote,
                Favorite = Favorite
            };
        }
    }
}