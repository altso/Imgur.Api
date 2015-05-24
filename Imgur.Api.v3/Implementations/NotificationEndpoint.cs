using System;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class NotificationEndpoint : INotificationEndpoint
    {
        private readonly IExecutor _executor;

        public NotificationEndpoint(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task<Notifications> GetNotifications()
        {
            return await _executor.ExecuteAsync<Notifications>(new RestRequest("notification").AddParameter("t", DateTime.UtcNow.Ticks), true);
        }

        public async Task<bool> MarkAsViewed(string id)
        {
            return await _executor.ExecuteAsync<bool>(new RestRequest("notification/{id}", Method.POST).AddUrlSegment("id", id), true);
        }
    }
}