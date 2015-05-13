namespace Imgur.Api.v3.Implementations
{
    public class AlbumOrImage : IAlbum, IImage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AccountUrl { get; set; }
        public string Privacy { get; set; }
        public string Cover { get; set; }
        public int Order { get; set; }
        public string Layout { get; set; }
        public int Datetime { get; set; }
        public string Link { get; set; }
        public string Gifv { get; set; }
        public string Mp4 { get; set; }
        public string Webm { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public int Views { get; set; }
        public long Bandwidth { get; set; }
        public string Deletehash { get; set; }
        public bool IsAlbum { get; set; }

        public Item ToItem()
        {
            if (IsAlbum) return ToAlbum();
            return ToImage();
        }

        private Album ToAlbum()
        {
            return new Album
            {
                Id = Id,
                Title = Title,
                Description = Description,
                AccountUrl = AccountUrl,
                Privacy = Privacy,
                Cover = Cover,
                Order = Order,
                Layout = Layout,
                Datetime = Datetime,
                Link = Link
            };
        }

        private Image ToImage()
        {
            return new Image
            {
                Id = Id,
                Link = Link,
                Title = Title,
                Description = Description,
                Datetime = Datetime,
                Type = Type,
                Animated = Animated,
                Width = Width,
                Height = Height,
                Size = Size,
                Views = Views,
                Bandwidth = Bandwidth,
                Deletehash = Deletehash,
                Gifv = Gifv,
                Mp4 = Mp4,
                Webm = Webm
            };
        }
    }
}