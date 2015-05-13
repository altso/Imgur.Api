using System;

namespace Imgur.Api.v3
{
    public class RateLimit
    {
        public int UserLimit { get; set; }
        public int UserRemaining { get; set; }
        public int UserReset { get; set; }
        public int ClientLimit { get; set; }
        public int ClientRemaining { get; set; }

        public override string ToString()
        {
            return string.Format(
                "UserRemaining: {1} ({0} at {2}), ClientRemaining: {4} ({3})",
                UserLimit,
                UserRemaining,
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(UserReset).ToLocalTime(),
                ClientLimit,
                ClientRemaining);
        }
    }
}