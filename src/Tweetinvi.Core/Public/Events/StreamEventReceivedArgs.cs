namespace Tweetinvi.Events
{
    public class StreamEventReceivedArgs
    {
        public StreamEventReceivedArgs(string json)
        {
            Json = json;
        }

        public string Json { get; }
    }
}