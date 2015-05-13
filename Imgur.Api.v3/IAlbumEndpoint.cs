using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IAlbumEndpoint
    {
        Task<Album> Get(string id);
        Task<Album> Create(IEnumerable<string> ids, string title, string description, string coverId);
        Task Update(string id, IEnumerable<string> ids, string title, string description, string coverId);
        Task Delete(string id);
        Task<string> Favorite(string id);
    }
}