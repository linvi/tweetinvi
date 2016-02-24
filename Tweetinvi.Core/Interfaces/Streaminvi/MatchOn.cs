using System;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// For more information regarding how Twitter matches the FilteredStream filters please visit
    /// https://dev.twitter.com/streaming/overview/request-parameters.
    /// </summary>
    [Flags]
    public enum MatchOn
    {
        /// <summary>
        /// Nothing to match.
        /// </summary>
        None,

        /// <summary>
        /// Match on all the fields used by Twitter filter stream.
        /// </summary>
        Everything = 1,

        /// <summary>
        /// The tweet text matches a track you follow.
        /// </summary>
        TweetText = 2,

        /// <summary>
        /// The follower is the person who sent the tweet.
        /// </summary>
        Follower = 4,

        /// <summary>
        /// The tweet location has matched a location you follow.
        /// </summary>
        TweetLocation = 8,

        /// <summary>
        /// When a tweet is sent directly to a follower of your list.
        /// </summary>
        FollowerInReplyTo = 16,

        /// <summary>
        /// The tweet entities matches a track you follow.
        /// </summary>
        AllEntities = 32,

        /// <summary>
        /// The track matches the text contained within a URL of a link or a media.
        /// </summary>
        URLEntities = 64,

        /// <summary>
        /// The track matches the text contained within a Hashtag.
        /// </summary>
        HashTagEntities = 128,

        /// <summary>
        /// The track matches the text contained within a user mention.
        /// </summary>
        UserMentionEntities = 256
    }
}