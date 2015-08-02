using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters
{
    /// <summary>
    /// Available parameters to publish a Tweet 
    /// Visit https://dev.twitter.com/rest/reference/post/statuses/update for more information.
    /// </summary>
    public interface IPublishTweetOptionalParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The ID of an existing status that the update is in reply to.
        /// </summary>
        long? InReplyToTweetId { get; set; }

        /// <summary>
        /// An existing status that the update is in reply to.
        /// </summary>
        ITweetIdentifier InReplyToTweet { get; set; }

        /// <summary>
        /// Quote a tweet
        /// </summary>
        ITweet QuotedTweet { get; set; }

        /// <summary>
        /// A <a href="https://dev.twitter.com/overview/api/places">place</a> in the world.
        /// </summary>
        string PlaceId { get; set; }

        /// <summary>
        /// Coordinates indicating the position from where the tweet has been published.
        /// </summary>
        ICoordinates Coordinates { get; set; }

        /// <summary>
        /// Whether or not to put a pin on the exact coordinates a tweet has been sent from.
        /// </summary>
        bool? DisplayExactCoordinates { get; set; }

        /// <summary>
        /// A list of media_ids to associate with the Tweet. You may include up to 4 photos or 1 animated GIF or 1 video in a Tweet.
        /// </summary>
        List<long> MediaIds { get; set; }

        /// <summary>
        /// A list of media (uploaded or not) that need to be displayed within the tweet.
        /// </summary>
        List<IMedia> Medias { get; set; }

        /// <summary>
        /// A list of media binaries that need to be uploaded and then displayed within the tweet.
        /// </summary>
        List<byte[]> MediaBinaries { get; set; }

        /// <summary>
        /// If you upload Tweet media that might be considered sensitive content such as 
        /// nudity, violence, or medical procedures, you should set this value to true. 
        /// </summary>
        bool? PossiblySensitive { get; set; }

        /// <summary>
        /// Tweet's creator will not be populated. 
        /// Only the user id property will be available.
        /// </summary>
        bool? TrimUser { get; set; }
    }
}