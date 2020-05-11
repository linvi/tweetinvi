namespace Tweetinvi.Events
{
    /// <summary>
    /// Event informing that a message was received
    /// </summary>
    public class StreamEventReceivedArgs
    {
        public StreamEventReceivedArgs(string json)
        {
            Json = json;
        }

        public string Json { get; }
    }
}