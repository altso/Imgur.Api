using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class MessageEndpoint : IMessageEndpoint
    {
        private readonly IExecutor _executor;

        public MessageEndpoint(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<Message>> GetMessages()
        {
            return await _executor.ExecuteAsync<List<Message>>(
                new RestRequest("messages")
                    .AddParameter("_", Environment.TickCount)
                , true);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int id)
        {
            return await _executor.ExecuteAsync<List<Message>>(
                new RestRequest("message/{id}/thread")
                    .AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture))
                    .AddParameter("_", Environment.TickCount)
                , true) ?? Enumerable.Empty<Message>();
        }

        public Task<Message> GetMessage(int id)
        {
            return _executor.ExecuteAsync<Message>(
                new RestRequest("message/{id}")
                    .AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture))
                , true);
        }

        public async Task<int> SendMessage(string recipient, string body, string subject, int? parentId = null)
        {
            var request = new RestRequest("message", Method.POST)
                .AddParameter("recipient", recipient)
                .AddParameter("body", body);
            if (subject != null)
            {
                request = request.AddParameter("subject", subject);
            }
            if (parentId != null)
            {
                request = request.AddParameter("parent_id", parentId);
            }
            var message = await _executor.ExecuteAsync<Message>(request, true);
            return message.Id;
        }
    }
}