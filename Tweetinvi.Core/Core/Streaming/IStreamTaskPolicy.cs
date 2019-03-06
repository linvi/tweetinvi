namespace Tweetinvi.Core.Streaming
{
    public enum HandleStreamWebExceptionsBy
    {
        StoppingStream,
        StoppingStreamAndThrowing
    }

    public interface IStreamTaskPolicy
    {
        HandleStreamWebExceptionsBy HandleStreamWebExceptionsBy { get; set; }
    }
}
