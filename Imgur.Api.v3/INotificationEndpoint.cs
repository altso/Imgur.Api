using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface INotificationEndpoint
    {
        Task<Notifications> GetNotifications();
        Task<bool> MarkAsViewed(string id);
    }
}