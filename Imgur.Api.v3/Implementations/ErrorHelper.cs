using System;

namespace Imgur.Api.v3.Implementations
{
    public static class ErrorHelper
    {
        private const string AlbumNotFoundMessage = "No album was found with the ID";

        public static string NormalizeErrorMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return "Unknown Error";
            if (message.StartsWith(AlbumNotFoundMessage, StringComparison.OrdinalIgnoreCase)) return AlbumNotFoundMessage;
            return message;
        }
    }
}