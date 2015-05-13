namespace Imgur.Api.v3.Implementations
{
    public class Imgur : IImgur
    {
        public Imgur(IImgurApi api, IAccountEndpoint account, IGalleryEndpoint gallery, ICommentEndpoint comment, IImageEndpoint image, IAlbumEndpoint album, INotificationEndpoint notification, IMessageEndpoint message)
        {
            Api = api;
            Account = account;
            Gallery = gallery;
            Comment = comment;
            Image = image;
            Album = album;
            Notification = notification;
            Message = message;
        }

        public IImgurApi Api { get; private set; }
        public IAccountEndpoint Account { get; private set; }
        public IGalleryEndpoint Gallery { get; private set; }
        public ICommentEndpoint Comment { get; private set; }
        public IImageEndpoint Image { get; private set; }
        public IAlbumEndpoint Album { get; private set; }
        public INotificationEndpoint Notification { get; private set; }
        public IMessageEndpoint Message { get; private set; }
    }
}