using Tweetinvi.Events;

namespace Tweetinvi.Events
{
    public class UserReadMessageConversationEventArgs : MessageConversationEventArgs
    {
        public string LastReadEventId { get; set; }
    }
}
