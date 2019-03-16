namespace Tweetinvi.Events
{
    public enum TweetDeletedRaisedInResultOf
    {
        /// <summary>
        /// The tweet was deleted by the account user.
        /// </summary>
        AccountUserDeletingOneOfHisTweets,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetDeleted event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown,
    }

    public class AccountActivityTweetDeletedEventArgs : BaseAccountActivityEventArgs<long>
    {
        public AccountActivityTweetDeletedEventArgs(AccountActivityEvent<long> activityEvent) : base(activityEvent)
        {
            TweetId = activityEvent.Args;

            InResultOf = TweetDeletedRaisedInResultOf.AccountUserDeletingOneOfHisTweets;
        }

        public long TweetId { get; }

        public TweetDeletedRaisedInResultOf InResultOf { get; }
    }
}
