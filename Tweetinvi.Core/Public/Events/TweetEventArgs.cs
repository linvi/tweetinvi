using System;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class TweetEventArgs : EventArgs
    {
        public TweetEventArgs(ITweet tweet, string json)
        {
            Tweet = tweet;
            Json = json;
        }

        public ITweet Tweet { get; private set; }
        public string Json { get; private set; }
    }

    public enum TweetCreatedBy
    {
        /// <summary>
        /// The tweet was created by the account user.
        /// </summary>
        AccountUser,

        /// <summary>
        /// The tweet has been created by another user in reply to a tweet posted by the account user.
        /// </summary>
        AnotherUserReplyingToAccountUser,

        /// <summary>
        /// The tweet has been created by another user and is mentioning the account user.
        /// </summary>
        AnotherUserMentioningTheAccountUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the TweetCreated event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown,
    }

    public class TweetReceivedEventArgs : TweetEventArgs
    {
        public TweetReceivedEventArgs(ITweet tweet, string json) : base(tweet, json)
        {
        }
    }

    public class TweetCreatedEventArgs : TweetReceivedEventArgs
    {
        public TweetCreatedEventArgs(ITweet tweet, string json, TweetCreatedBy createdBy) : base(tweet, json)
        {
            CreatedBy = createdBy;
        }

        public TweetCreatedBy CreatedBy { get; }
    }

    public class TweetFavouritedEventArgs : TweetEventArgs
    {
        public TweetFavouritedEventArgs(ITweet tweet, string json, IUser favoritingUser) : base(tweet, json)
        {
            FavouritingUser = favoritingUser;
        }

        public IUser FavouritingUser { get; private set; }
    }

    public class MatchedTweetReceivedEventArgs : TweetEventArgs
    {
        public MatchedTweetReceivedEventArgs(ITweet tweet, string json) : base(tweet, json)
        {
        }

        public string[] MatchingTracks { get; set; }
        public ILocation[] MatchingLocations { get; set; }
        public long[] MatchingFollowers { get; set; }
        public MatchOn MatchOn { get; set; }


        public string[] QuotedTweetMatchingTracks { get; set; }
        public ILocation[] QuotedTweetMatchingLocations { get; set; }
        public long[] QuotedTweetMatchingFollowers { get; set; }
        public MatchOn QuotedTweetMatchOn { get; set; }
    }

    public class TweetDeletedEventArgs : EventArgs
    {
        public long TweetId { get; set; }
        public long UserId { get; set; }
        public long? Timestamp { get; set; }
    }

    public class TweetLocationDeletedEventArgs : EventArgs
    {
        public TweetLocationDeletedEventArgs(ITweetLocationRemovedInfo tweetLocationRemovedInfo)
        {
            TweetLocationRemovedInfo = tweetLocationRemovedInfo;
        }

        public ITweetLocationRemovedInfo TweetLocationRemovedInfo { get; private set; }
    }

    public class TweetWitheldEventArgs : EventArgs
    {
        public TweetWitheldEventArgs(ITweetWitheldInfo tweetWitheldInfo)
        {
            TweetWitheldInfo = tweetWitheldInfo;
        }

        public ITweetWitheldInfo TweetWitheldInfo { get; private set; }
    }
}