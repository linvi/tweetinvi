using Tweetinvi.Core.Streaming;

namespace Tweetinvi.Streams
{
    public class StreamTaskPolicy : IStreamTaskPolicy
    {
        public StreamTaskPolicy()
        {
            HandleStreamWebExceptionsBy = HandleStreamWebExceptionsBy.StoppingStreamAndThrowing;
        }

        public HandleStreamWebExceptionsBy HandleStreamWebExceptionsBy { get; set; }
    }
}
