using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/statuses/update
    /// </summary>
    public interface IPublishTweetParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Message to publish as a twwweet
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Optional parameters to publish the tweet
        /// </summary>
        IPublishTweetOptionalParameters Parameters { get; }

        #region Copy of IPublishTweetOptionalParameters

        // This is a copy so that developers cannot create a IPublishTweet parameter as 
        // a IPublishTweetOptionalParameters in Tweet.PublishTweet for example

        /// <summary>
        /// The ID of an existing status that the update is in reply to.
        /// </summary>
        long? InReplyToTweetId { get; set; }

        /// <summary>
        /// Quote a specific tweet
        /// </summary>
        ITweet QuotedTweet { get; set; }

        /// <summary>
        /// An existing status that the update is in reply to.
        /// </summary>
        ITweetIdentifier InReplyToTweet { get; set; }

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
        List<long> MediaIds { get; }

        /// <summary>
        /// A list of media (uploaded or not) that need to be displayed within the tweet.
        /// </summary>
        List<IMedia> Medias { get; }

        /// <summary>
        /// A list of media binaries that need to be uploaded and then displayed within the tweet.
        /// </summary>
        List<byte[]> MediaBinaries { get; }

        /// <summary>
        /// If you upload Tweet media that might be considered sensitive content such as 
        /// nudity, violence, or medical procedures, you should set this value to true. 
        /// </summary>
        bool? PossiblySensitive { get; set; }

        /// <summary>
        /// If set to true, the creator property (IUser) will only contain the id.
        /// </summary>
        bool? TrimUser { get; set; }

        #endregion
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/statuses/update
    /// </summary>
    public class PublishTweetParameters : IPublishTweetParameters
    {
        public PublishTweetParameters(string text, IPublishTweetOptionalParameters optionalParameters = null)
        {
            Text = text;
            Parameters = optionalParameters ?? new PublishTweetOptionalParameters();
        }

        public string Text { get; private set; }
        public IPublishTweetOptionalParameters Parameters { get; private set; }

        public long? InReplyToTweetId
        {
            get { return Parameters.InReplyToTweetId; }
            set { Parameters.InReplyToTweetId = value; }
        }

        public ITweetIdentifier InReplyToTweet
        {
            get { return Parameters.InReplyToTweet; }
            set { Parameters.InReplyToTweet = value; }
        }

        public string PlaceId
        {
            get { return Parameters.PlaceId; }
            set { Parameters.PlaceId = value; }
        }

        public ICoordinates Coordinates
        {
            get { return Parameters.Coordinates; }
            set { Parameters.Coordinates = value; }
        }

        public ITweet QuotedTweet
        {
            get { return Parameters.QuotedTweet; }
            set { Parameters.QuotedTweet = value; }
        }

        public bool? DisplayExactCoordinates
        {
            get { return Parameters.DisplayExactCoordinates; }
            set { Parameters.DisplayExactCoordinates = value; }
        }

        public List<long> MediaIds
        {
            get { return Parameters.MediaIds; }
        }

        public List<IMedia> Medias
        {
            get { return Parameters.Medias; }
        }

        public List<byte[]> MediaBinaries
        {
            get { return Parameters.MediaBinaries; }
        }

        public bool? PossiblySensitive
        {
            get { return Parameters.PossiblySensitive; }
            set { Parameters.PossiblySensitive = value; }
        }

        public bool? TrimUser
        {
            get { return Parameters.TrimUser; }
            set { Parameters.TrimUser = value; }
        }

        public List<Tuple<string, string>> CustomQueryParameters
        {
            get { return Parameters.CustomQueryParameters; }
        }

        public string FormattedCustomQueryParameters
        {
            get { return Parameters.FormattedCustomQueryParameters; }
        }

        public void AddCustomQueryParameter(string name, string value)
        {
            Parameters.AddCustomQueryParameter(name, value);
        }

        public void ClearCustomQueryParameters()
        {
            Parameters.ClearCustomQueryParameters();
        }
    }
}