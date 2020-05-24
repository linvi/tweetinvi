namespace Tweetinvi.Events
{
    public enum TweetDeletedRaisedInResultOf
    {
        /// <summary>
        /// The tweet was deleted by another user
        /// </summary>
        AnotherUserDeletedATweet = 0,

        /// <summary>
        /// The tweet was deleted by the account user.
        /// </summary>
        AccountUserDeletingOneOfHisTweets,
    }

    /// <summary>
    /// Event information when a tweet is deleted.
    /// </summary>
    public class TweetDeletedEvent : BaseAccountActivityEventArgs<TweetDeletedRaisedInResultOf>
    {
        public TweetDeletedEvent(AccountActivityEvent<long> activityEvent, long userId) : base(activityEvent)
        {
            TweetId = activityEvent.Args;
            UserId = userId;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// Id of the tweet that was deleted
        /// </summary>
        public long TweetId { get; }

        /// <summary>
        /// Id of the user who created the tweet
        /// </summary>
        public long UserId { get; }

        private TweetDeletedRaisedInResultOf GetInResultOf()
        {
            if (UserId == AccountUserId)
            {
                return TweetDeletedRaisedInResultOf.AccountUserDeletingOneOfHisTweets;
            }

            return TweetDeletedRaisedInResultOf.AnotherUserDeletedATweet;
        }
    }
}
