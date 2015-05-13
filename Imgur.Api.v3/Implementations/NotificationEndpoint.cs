using System;
using System.Threading.Tasks;
using RestSharp;

namespace Imgur.Api.v3.Implementations
{
    public class NotificationEndpoint : INotificationEndpoint
    {
        private readonly IImgurApi _imgurApi;

        public NotificationEndpoint(IImgurApi imgurApi)
        {
            _imgurApi = imgurApi;
        }

        public async Task<Notifications> GetNotifications()
        {
            return await _imgurApi.ExecuteAsync<Notifications>(new RestRequest("notification").AddParameter("t", DateTime.UtcNow.Ticks), true);
        }

        public async Task<bool> MarkAsViewed(string id)
        {
            return await _imgurApi.ExecuteAsync<bool>(new RestRequest("notification/{id}", Method.POST).AddUrlSegment("id", id), true);
        }
    }
}