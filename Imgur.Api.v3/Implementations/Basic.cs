namespace Imgur.Api.v3.Implementations
{
    public class Basic<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }
}