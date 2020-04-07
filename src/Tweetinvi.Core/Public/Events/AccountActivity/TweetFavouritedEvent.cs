using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum TweetFavouritedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetFavourited event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user favourited one of his own tweet
        /// </summary>
        AccountUserFavouritingHisOwnTweet,

        /// <summary>
        /// The account user facourited a tweet of another user
        /// </summary>
        AccountUserFavouritingATweetOfAnotherUser,

        /// <summary>
        /// Another user favourited the tweet of the account user
        /// </summary>
        AnotherUserFavouritingATweetOfTheAccountUser,
    }

    /// <summary>
    /// Event information when a tweet is favourited.
    /// </summary>
    public class TweetFavouritedEvent : BaseAccountActivityEventArgs<TweetFavouritedRaisedInResultOf>
    {
        public TweetFavouritedEvent(AccountActivityEvent<Tuple<ITweet, IUser>> eventInfo) : base(eventInfo)
        {
            Tweet = eventInfo.Args.Item1;
            FavouritedBy = eventInfo.Args.Item2;
            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The tweet that got favourited
        /// </summary>
        public ITweet Tweet { get; }

        /// <summary>
        /// The user who favourited the tweet
        /// </summary>
        public IUser FavouritedBy { get; }

        private TweetFavouritedRaisedInResultOf GetInResultOf()
        {
            if (FavouritedBy.Id == AccountUserId && Tweet.CreatedBy.Id == AccountUserId)
            {
                return TweetFavouritedRaisedInResultOf.AccountUserFavouritingHisOwnTweet;
            }

            if (FavouritedBy.Id == AccountUserId)
            {
                return TweetFavouritedRaisedInResultOf.AccountUserFavouritingATweetOfAnotherUser;
            }

            if (Tweet.CreatedBy.Id == AccountUserId)
            {
                return TweetFavouritedRaisedInResultOf.AnotherUserFavouritingATweetOfTheAccountUser;
            }

            return TweetFavouritedRaisedInResultOf.Unknown;
        }
    }
}
