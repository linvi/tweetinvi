using System;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// For more information regarding how Twitter matches the FilteredStream filters please visit
    /// https://dev.twitter.com/streaming/overview/request-parameters.
    /// </summary>
    [Flags]
    public enum StreamMatchType
    {
        /// <summary>
        /// Nothing to match.
        /// </summary>
        None,

        /// <summary>
        /// The tweet text matches a track you follow.
        /// </summary>
        TweetText = 1,

        /// <summary>
        /// The follower is the person who sent the tweet.
        /// </summary>
        Follower = 2,

        /// <summary>
        /// The tweet location has matched a location you follow.
        /// </summary>
        TweetLocation = 4,

        /// <summary>
        /// When a tweet is sent directly to a follower of your list.
        /// </summary>
        FollowerInReplyTo = 8,

        /// <summary>
        /// The tweet entities matches a track you follow.
        /// </summary>
        AllEntities = 16,

        /// <summary>
        /// The track matches the text contained within a URL of a link or a media.
        /// </summary>
        URLEntity = 32,

        /// <summary>
        /// The track matches the text contained within a Hashtag.
        /// </summary>
        HashTagEntity = 64,

        /// <summary>
        /// The track matches the text contained within a user mention.
        /// </summary>
        UserMentionEntity = 128
    }
}