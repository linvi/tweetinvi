using System;
using System.Collections.Generic;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Models;

namespace Tweetinvi.Streams
{
    public interface IFilterStreamTweetMatcherFactory
    {
        IFilterStreamTweetMatcher Create(
            IStreamTrackManager<ITweet> streamTrackManager,
            Dictionary<ILocation, Action<ITweet>> locations,
            Dictionary<long?, Action<ITweet>> followingUserIds);
    }

    public class FilterStreamTweetMatcherFactory : IFilterStreamTweetMatcherFactory
    {
        public IFilterStreamTweetMatcher Create(
            IStreamTrackManager<ITweet> streamTrackManager, 
            Dictionary<ILocation, Action<ITweet>> locations,
            Dictionary<long?, Action<ITweet>> followingUserIds)
        {
            return new FilterStreamTweetMatcher(streamTrackManager, locations, followingUserIds);
        }
    }
}
