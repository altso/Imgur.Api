using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Imgur.Api.v3
{
    public enum ImageSize
    {
        /// <summary>
        /// Original untouched image.
        /// </summary>
        Original,

        /// <summary>
        /// 90x90 without proportions.
        /// </summary>
        SmallSquare,

        /// <summary>
        /// 160x160 without proportions.
        /// </summary>
        BigSquare,

        /// <summary>
        /// 160x160 with proportions.
        /// </summary>
        SmallThumbnail,

        /// <summary>
        /// 320x320 with proportions.
        /// </summary>
        MediumThumbnail,

        /// <summary>
        /// 640x640 with proportions.
        /// </summary>
        LargeThumbnail,

        /// <summary>
        /// 1024x1024 with proportions.
        /// </summary>
        HugeThumbnail,
    }

    public static class ImgurUri
    {
        public const string MimeTypeJpg = "image/jpeg";
        public const string MimeTypePng = "image/png";
        public const string MimeTypeGif = "image/gif";

        private static readonly Dictionary<ImageSize, string> UrlFormats = new Dictionary<ImageSize, string>
        {
            { ImageSize.Original, "http://i.imgur.com/{0}{1}" },
            { ImageSize.SmallSquare, "http://i.imgur.com/{0}s{1}" },
            { ImageSize.BigSquare, "http://i.imgur.com/{0}b{1}" },
            { ImageSize.SmallThumbnail, "http://i.imgur.com/{0}t{1}" },
            { ImageSize.MediumThumbnail, "http://i.imgur.com/{0}m{1}" },
            { ImageSize.LargeThumbnail, "http://i.imgur.com/{0}l{1}" },
            { ImageSize.HugeThumbnail, "http://i.imgur.com/{0}h{1}" },
        };

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            { MimeTypeJpg, ".jpg" },
            { MimeTypePng, ".png" },
            { MimeTypeGif, ".gif" },
        };

        public static Uri ForImage(string hash, string mimeType, ImageSize imageSize)
        {
            string extension;
            if (!MimeTypes.TryGetValue(mimeType, out extension))
            {
                Debug.WriteLine("Unrecognized mime type '{0}'", mimeType);
            }

            return new Uri(string.Format(UrlFormats[imageSize], hash, extension), UriKind.Absolute);
        }

        public static Uri ForGalleryPage(string hash)
        {
            return new Uri(string.Format("http://imgur.com/gallery/{0}", hash));
        }

        public static Uri ForImagePage(string hash)
        {
            return new Uri(string.Format("http://imgur.com/{0}", hash));
        }

        public static Uri ForAlbumPage(string hash)
        {
            return new Uri(string.Format("http://imgur.com/a/{0}", hash));
        }

        public static Uri ForImage(IImage image)
        {
            return ForImage(image.Id, image.Type, ImageSize.HugeThumbnail);
        }

        public static Uri ForDelete(string deleteHash)
        {
            return new Uri(string.Format("http://imgur.com/delete/{0}", deleteHash));
        }
    }
}