﻿using System.Threading.Tasks;
using Imgur.Api.v3.Http;

namespace Imgur.Api.v3.Implementations
{
    public class CommentEndpoint : ICommentEndpoint
    {
        private readonly IImgurApi _imgurApi;

        public CommentEndpoint(IImgurApi imgurApi)
        {
            _imgurApi = imgurApi;
        }

        public async Task Vote(string id, string vote)
        {
            await _imgurApi.ExecuteAsync<bool>(
                new RestRequest("comment/{id}/vote/{vote}", Method.POST)
                    .AddUrlSegment("id", id)
                    .AddUrlSegment("vote", vote),
                true).ConfigureAwait(false);
        }

        public async Task<string> Create(string imageId, string comment, string parentId)
        {
            var result = await _imgurApi.ExecuteAsync<CommentItem>(
                new RestRequest("comment", Method.POST)
                    .AddParameter("image_id", imageId)
                    .AddParameter("comment", comment)
                    .AddParameter("parent_id", parentId),
                true).ConfigureAwait(false);
            if (result != null)
            {
                return result.Id;
            }
            throw new ImgurException("You are commenting too fast. Please try again later.");
        }

        public async Task<CommentItem> Get(string id)
        {
            var result = await _imgurApi.ExecuteAsync<CommentItem>(
                new RestRequest("comment/{id}")
                    .AddUrlSegment("id", id),
                true).ConfigureAwait(false);
            return result;
        }
    }
}