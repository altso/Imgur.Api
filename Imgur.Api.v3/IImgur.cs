namespace Imgur.Api.v3
{
    public interface IImgur
    {
        IImgurApi Api { get; }
        IAccountEndpoint Account { get; }
        IGalleryEndpoint Gallery { get; }
        ICommentEndpoint Comment { get; }
        IImageEndpoint Image { get; }
        IAlbumEndpoint Album { get; }
        INotificationEndpoint Notification { get; }
        IMessageEndpoint Message { get; }
    }
}