namespace Tweetinvi.Events
{
    public enum TweetDeletedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetDeleted event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The tweet was deleted by the account user.
        /// </summary>
        AccountUserDeletingOneOfHisTweets,
    }

    public class AccountActivityTweetDeletedEventArgs : BaseAccountActivityEventArgs<TweetDeletedRaisedInResultOf>
    {
        public AccountActivityTweetDeletedEventArgs(AccountActivityEvent<long> activityEvent, long userId) : base(activityEvent)
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

            return TweetDeletedRaisedInResultOf.Unknown;
        }
    }
}
