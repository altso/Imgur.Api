using System;

namespace Imgur.Api.v3
{
    public class OfflineException : Exception
    {
        public OfflineException()
            : base("No Internet connection, try again...")
        {
        }
    }
}