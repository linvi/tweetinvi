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

    public class TweetReceivedEventArgs : TweetEventArgs
    {
        public TweetReceivedEventArgs(ITweet tweet, string json) : base(tweet, json)
        {
            Json = json;
        }

        public string Json { get; set; }
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
    }

    public class TweetDeletedEventArgs : EventArgs
    {
        public TweetDeletedEventArgs(ITweetDeletedInfo tweetDeletedInfo)
        {
            TweetDeletedInfo = tweetDeletedInfo;
        }

        public ITweetDeletedInfo TweetDeletedInfo { get; private set; }
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