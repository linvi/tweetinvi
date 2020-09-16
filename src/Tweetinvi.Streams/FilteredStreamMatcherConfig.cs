using System;
using System.Collections.Generic;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Tweetinvi.Streams
{
    public class FilteredStreamMatcherConfig<T>
    {
        public MatchOn MatchOn { get; }
        public Dictionary<T, Action<ITweet>> TweetMatchingTrackAndActions { get; }
        public Dictionary<T, Action<ITweet>> RetweetMatchingTrackAndActions { get; }
        public Dictionary<T, Action<ITweet>> QuotedTweetMatchingTrackAndActions { get; }

        public FilteredStreamMatcherConfig(MatchOn matchOn)
        {
            MatchOn = matchOn;
            TweetMatchingTrackAndActions = new Dictionary<T, Action<ITweet>>();
            RetweetMatchingTrackAndActions = new Dictionary<T, Action<ITweet>>();
            QuotedTweetMatchingTrackAndActions = new Dictionary<T, Action<ITweet>>();
        }

        public Dictionary<T, Action<ITweet>> GetAllMatchingTracks()
        {
            return TweetMatchingTrackAndActions
                .MergeWith(RetweetMatchingTrackAndActions)
                .MergeWith(QuotedTweetMatchingTrackAndActions);
        }
    }
}