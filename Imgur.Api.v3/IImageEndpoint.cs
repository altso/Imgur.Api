using System;
using System.IO;
using System.Threading.Tasks;

namespace Imgur.Api.v3
{
    public interface IImageEndpoint
    {
        Task<Image> Get(string id);
        Task Update(string id, string title, string description);
        Task Delete(string id);
        Task<string> Favorite(string id);
        Task<Image> Upload(Stream image, string albumId, string name, string title, string description, IProgress<double> progress);
    }
}