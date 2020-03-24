using System;

namespace Tweetinvi.Events
{
    public class TweetDeletedEventArgs : EventArgs
    {
        public long TweetId { get; set; }
        public long UserId { get; set; }
        public long? Timestamp { get; set; }
    }
}
