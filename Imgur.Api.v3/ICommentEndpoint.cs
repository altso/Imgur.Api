using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface ICommentEndpoint
    {
        Task Vote(string id, string vote);
        Task<string> Create(string imageId, string comment, string parentId);
        Task<CommentItem> Get(string id);
    }
}