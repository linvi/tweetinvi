﻿using System;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
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

        /// <summary>
        /// Twitter will auto-populate the @mentions in the extended tweet prefix from the Tweet
        /// being replied to, plus a mention of the screen name that posted the Tweet being replied to.
        /// i.e. This auto-populates a "reply all".
        /// Must be used with InReplyToTweetId or InReplyToTweet.
        /// Use ExcludeReplyUserIds to specify accounts to not mention in the prefix.
        /// Also note that there can be a maximum of 50 mentions in the prefix, any more will error.
        /// </summary>
        bool? AutoPopulateReplyMetadata { get; set; }

        /// <summary>
        /// Twitter User IDs to not include in the auto-populated extended Tweet prefix.
        /// Cannot exclude the User who is directly being replied to, only the additional mentions.
        /// Must be used with AutoPopulateReplyMetadata.
        /// </summary>
        IEnumerable<long> ExcludeReplyUserIds { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/statuses/update
    /// </summary>
    public class PublishTweetOptionalParameters : CustomRequestParameters, IPublishTweetOptionalParameters
    {
        private ITweetIdentifier _tweetIdentifier;

        public PublishTweetOptionalParameters()
        {
            MediaIds = new List<long>();
            Medias = new List<IMedia>();
            MediaBinaries = new List<byte[]>();
        }

        public ITweetIdentifier InReplyToTweet
        {
            get { return _tweetIdentifier; }
            set
            {
                if (value != null && value.Id == -1)
                {
                    throw new InvalidOperationException("You cannot reply to a tweet that has not yet been published!");
                }

                _tweetIdentifier = value;
            }
        }

        public ITweet QuotedTweet { get; set; }

        public long? InReplyToTweetId
        {
            get { return InReplyToTweet != null ? (long?)InReplyToTweet.Id : null; }
            set
            {
                if (value != null)
                {
                    InReplyToTweet = new TweetIdentifier((long)value);
                }
                else
                {
                    InReplyToTweet = null;
                }
            }
        }

        public List<long> MediaIds { get; set; }
        public List<IMedia> Medias { get; set; }
        public List<byte[]> MediaBinaries { get; set; }

        public string PlaceId { get; set; }
        public ICoordinates Coordinates { get; set; }
        public bool? DisplayExactCoordinates { get; set; }

        public bool? PossiblySensitive { get; set; }
        public bool? TrimUser { get; set; }
        public bool? AutoPopulateReplyMetadata { get; set; }
        public IEnumerable<long> ExcludeReplyUserIds { get; set; }
    }
}