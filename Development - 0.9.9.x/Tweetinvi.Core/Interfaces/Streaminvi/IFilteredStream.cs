using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface IFilteredStream : ITwitterStream, ITrackableStream<ITweet>
    {
        event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;
        event EventHandler<MatchedTweetAndLocationReceivedEventArgs> MatchingTweetAndLocationReceived;

        void StartStreamMatchingAnyCondition();
        void StartStreamMatchingAllConditions();

        Task StartStreamMatchingAnyConditionAsync();
        Task StartStreamMatchingAllConditionsAsync();

        #region Follow
        /// <summary>
        /// List of UserId followed by the stream
        /// </summary>
        Dictionary<long?, Action<ITweet>> FollowingUserIds { get; }

        /// <summary>
        /// Follow a specific userId
        /// </summary>
        void AddFollow(long? userId, Action<ITweet> userPublishedTweet = null);

        /// <summary>
        /// Follow a specific user
        /// </summary>
        void AddFollow(IUser user, Action<ITweet> userPublishedTweet = null);

        /// <summary>
        /// Unfollow a specific userId
        /// </summary>
        void RemoveFollow(long? userId);

        /// <summary>
        /// Unfollow a specific user
        /// </summary>
        void RemoveFollow(IUser user);

        /// <summary>
        /// Tells you whether you are following a userId
        /// </summary>
        bool ContainsFollow(long? userId);

        /// <summary>
        /// Tells you whether you are following a user
        /// </summary>
        bool ContainsFollow(IUser user);

        /// <summary>
        /// Unfollow all the currently followed users
        /// </summary>
        void ClearFollows();
        #endregion

        #region Location

        /// <summary>
        /// List of locations analyzed by the stream
        /// </summary>
        Dictionary<ILocation, Action<ITweet>> Locations { get; }

        /// <summary>
        /// Add a location for the stream to analyze
        /// </summary>
        void AddLocation(ILocation location, Action<ITweet> locationDetected = null);

        /// <summary>
        /// Add a location for the stream to analyze
        /// </summary>
        ILocation AddLocation(ICoordinates coordinate1, ICoordinates coordinate2, Action<ITweet> locationDetected = null);

        /// <summary>
        /// Remove a location for the stream to analyze
        /// </summary>
        void RemoveLocation(ILocation location);

        /// <summary>
        /// Remove a location for the stream to analyze
        /// </summary>
        void RemoveLocation(ICoordinates coordinate1, ICoordinates coordinate2);

        /// <summary>
        /// Tells you whether you are analyzing a specific location
        /// </summary>
        bool ContainsLocation(ILocation location);

        /// <summary>
        /// Tells you whether you are analyzing a specific location
        /// </summary>
        bool ContainsLocation(ICoordinates coordinate1, ICoordinates coordinate2);

        /// <summary>
        /// Remove all the currently analyzed locations
        /// </summary>
        void ClearLocations();

        #endregion
    }
}