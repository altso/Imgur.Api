using System;
using System.Net;

namespace Imgur.Api.v3
{
    public class ImgurException : Exception
    {
        public ImgurException()
        {
        }

        public ImgurException(string message)
            : base(message)
        {
        }

        public ImgurException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class RateLimitExceededException : ImgurException
    {
        public RateLimitExceededException()
            : base("Api rate limit is exceeded. Please try again later.")
        {
        }
    }

    public class NotOkException : ImgurException
    {
        public NotOkException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}