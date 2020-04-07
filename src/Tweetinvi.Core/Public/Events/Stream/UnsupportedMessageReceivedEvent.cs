namespace Tweetinvi.Events
{
    public class UnsupportedMessageReceivedEvent
    {
        public UnsupportedMessageReceivedEvent(string json)
        {
            JsonMessageReceived = json;
        }

        public string JsonMessageReceived { get; }
    }
}