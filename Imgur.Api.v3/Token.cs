using System;

namespace Imgur.Api.v3
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
        public string AccountUsername { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("AccessToken: {0}, ExpiresIn: {1}, RefreshToken: {2}, Scope: {3}, TokenType: {4}", AccessToken, ExpiresIn, RefreshToken, Scope, TokenType);
        }
    }
}