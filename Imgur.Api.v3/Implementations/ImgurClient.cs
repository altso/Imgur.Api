using System;
using System.Threading.Tasks;

namespace Imgur.Api.v3.Implementations
{
    public class ImgurClient : IImgur
    {
        public ImgurClient(string clientId, string clientSecret, Func<Uri, Task<string>> authorizer, Token token)
        {
            var api = new ImgurApi(clientId, clientSecret, authorizer, token);
            Api = api;
            Account = new AccountEndpoint(api);
            Gallery = new GalleryEndpoint(api);
            Comment = new CommentEndpoint(api);
            Image = new ImageEndpoint(api);
            Album = new AlbumEndpoint(api);
            Notification = new NotificationEndpoint(api);
            Message = new MessageEndpoint(api);
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