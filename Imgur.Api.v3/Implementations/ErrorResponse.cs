namespace Imgur.Api.v3.Implementations
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Method { get; set; }
        public string Parameters { get; set; }
        public string Request { get; set; }
    }
}