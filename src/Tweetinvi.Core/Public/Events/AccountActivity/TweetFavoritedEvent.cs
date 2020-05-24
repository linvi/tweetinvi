using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum TweetFavoritedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetFavorited event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user Favorited one of his own tweet
        /// </summary>
        AccountUserFavoritingHisOwnTweet,

        /// <summary>
        /// The account user facourited a tweet of another user
        /// </summary>
        AccountUserFavoritingATweetOfAnotherUser,

        /// <summary>
        /// Another user Favorited the tweet of the account user
        /// </summary>
        AnotherUserFavoritingATweetOfTheAccountUser,
    }

    /// <summary>
    /// Event information when a tweet is Favorited.
    /// </summary>
    public class TweetFavoritedEvent : BaseAccountActivityEventArgs<TweetFavoritedRaisedInResultOf>
    {
        public TweetFavoritedEvent(AccountActivityEvent<Tuple<ITweet, IUser>> eventInfo) : base(eventInfo)
        {
            Tweet = eventInfo.Args.Item1;
            FavoritedBy = eventInfo.Args.Item2;
            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The tweet that got Favorited
        /// </summary>
        public ITweet Tweet { get; }

        /// <summary>
        /// The user who Favorited the tweet
        /// </summary>
        public IUser FavoritedBy { get; }

        private TweetFavoritedRaisedInResultOf GetInResultOf()
        {
            if (FavoritedBy.Id == AccountUserId && Tweet.CreatedBy.Id == AccountUserId)
            {
                return TweetFavoritedRaisedInResultOf.AccountUserFavoritingHisOwnTweet;
            }

            if (FavoritedBy.Id == AccountUserId)
            {
                return TweetFavoritedRaisedInResultOf.AccountUserFavoritingATweetOfAnotherUser;
            }

            if (Tweet.CreatedBy.Id == AccountUserId)
            {
                return TweetFavoritedRaisedInResultOf.AnotherUserFavoritingATweetOfTheAccountUser;
            }

            return TweetFavoritedRaisedInResultOf.Unknown;
        }
    }
}
