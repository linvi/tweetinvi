using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum TweetFavouritedRaisedInResultOf
    {
        /// <summary>
        /// The account user favourited one of his own tweet
        /// </summary>
        AccountUserFavouritingHisOwnTweet,

        /// <summary>
        /// 
        /// </summary>
        AccountUserFavouritingATweetOfAnotherUser,

        /// <summary>
        /// 
        /// </summary>
        AnotherUserFavouritingATweetOfTheAccountUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetFavourited event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown,
    }

    public class AccountActivityTweetFavouritedEventArgs : BaseAccountActivityEventArgs<TweetFavouritedRaisedInResultOf>
    {
        public AccountActivityTweetFavouritedEventArgs(AccountActivityEvent<Tuple<ITweet, IUser>> eventInfo) : base(eventInfo)
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
