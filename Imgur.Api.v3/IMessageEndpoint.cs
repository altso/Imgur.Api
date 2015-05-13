using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IMessageEndpoint
    {
        Task<IEnumerable<Message>> GetMessages();

        Task<IEnumerable<Message>> GetMessageThread(int id);

        Task<Message> GetMessage(int id);

        Task<int> SendMessage(string recipient, string body, string subject, int? parentId = null);
    }
}