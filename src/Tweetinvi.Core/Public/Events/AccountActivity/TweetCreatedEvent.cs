using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum TweetCreatedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetCreated event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The tweet was created by the account user.
        /// </summary>
        AccountUserCreatingATweet,

        /// <summary>
        /// The tweet has been created by another user in reply to a tweet posted by the account user.
        /// </summary>
        AnotherUserReplyingToAccountUser,

        /// <summary>
        /// The tweet has been created by another user and is mentioning the account user.
        /// </summary>
        AnotherUserMentioningTheAccountUser,
    }

    /// <summary>
    /// Event information when a tweet is published.
    /// </summary>
    public class TweetCreatedEvent : BaseAccountActivityEventArgs<TweetCreatedRaisedInResultOf>
    {
        public TweetCreatedEvent(AccountActivityEvent<ITweet> eventInfo) : base(eventInfo)
        {
            Tweet = eventInfo.Args;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The tweet that was published
        /// </summary>
        public ITweet Tweet { get; }

        private TweetCreatedRaisedInResultOf GetInResultOf()
        {
            if (Tweet.CreatedBy.Id == AccountUserId)
            {
                return TweetCreatedRaisedInResultOf.AccountUserCreatingATweet;
            }

            if (Tweet.InReplyToStatusId != null && Tweet.InReplyToUserId == AccountUserId)
            {
                return TweetCreatedRaisedInResultOf.AnotherUserReplyingToAccountUser;
            }

            if (Tweet.InReplyToUserId == AccountUserId)
            {
                return TweetCreatedRaisedInResultOf.AnotherUserMentioningTheAccountUser;
            }

            return TweetCreatedRaisedInResultOf.Unknown;
        }
    }
}
