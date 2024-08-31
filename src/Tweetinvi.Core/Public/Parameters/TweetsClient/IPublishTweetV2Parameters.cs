using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Tweetinvi.Parameters.V2
{
    public interface IPublishTweetV2Parameters
    {
        [JsonProperty("text")]
        string Text { get; set; }

        /// <summary>
        ///     Tweets a link directly to a Direct Message conversation with an account.
        /// </summary>
        [JsonProperty("direct_message_deep_link", NullValueHandling = NullValueHandling.Ignore)]
        string? DirectMessageDeepLink { get; set; }

        /// <summary>
        ///     Allows you to Tweet exclusively for Super Followers.
        /// </summary>
        [JsonProperty("for_super_followers_only", NullValueHandling = NullValueHandling.Ignore)]
        bool? ForSuperFollowersOnly { get; set; }

        /// <summary>
        ///     A JSON object that contains information of the Tweet being replied to.
        /// </summary>
        [JsonProperty("reply", NullValueHandling = NullValueHandling.Ignore)]
        IPublishTweetV2ReplyParameters? Reply { get; set; }

        /// <summary>
        ///     A JSON object that contains media information being attached to created Tweet. This is mutually exclusive from Quote Tweet ID and Poll.
        /// </summary>
        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        IPublishTweetV2MediaParameter? Media { get; set; }

        /// <summary>
        ///     Link to the Tweet being quoted.
        /// </summary>
        [JsonProperty("quote_tweet_id", NullValueHandling = NullValueHandling.Ignore)]
        string? QuoteTweetId { get; set; }
    }

    /// <summary>
    ///     A JSON object that contains information of the Tweet being replied to.
    /// </summary>
    public interface IPublishTweetV2ReplyParameters
    {
        /// <summary>
        ///  Tweet ID of the Tweet being replied to. Please note that in_reply_to_tweet_id needs to be in the request if exclude_reply_user_ids is present.
        /// </summary>
        [JsonProperty("in_reply_to_tweet_id", NullValueHandling = NullValueHandling.Ignore)]
        string InReplyToTweetId { get; set; }

        /// <summary>
        ///     A list of User IDs to be excluded from the reply Tweet thus removing a user from a thread.
        /// </summary>
        [JsonProperty("exclude_reply_user_ids", NullValueHandling = NullValueHandling.Ignore)]
        string[] ExcludeReplyUserIds { get; set; }
    }

    /// <summary>
    ///     A JSON object that contains media information being attached to created Tweet. This is mutually exclusive from Quote Tweet ID and Poll.
    /// </summary>
    public interface IPublishTweetV2MediaParameter
    {
        /// <summary>
        ///     A list of Media IDs being attached to the Tweet. This is only required if the request includes the tagged_user_ids.
        /// </summary>
        [JsonProperty("media_ids")]
        string[] MediaIds { get; set; }

        /// <summary>
        ///     A list of User IDs being tagged in the Tweet with Media. 
        ///     If the user you're tagging doesn't have photo-tagging enabled, 
        ///     their names won't show up in the list of tagged users even though the Tweet is successfully created.
        /// </summary>
        [JsonProperty("tagged_user_ids")]
        string[] TaggedUserIds { get; set; }
    }

    public class PublishTweetV2ReplyParameters : IPublishTweetV2ReplyParameters
    {
        [JsonProperty("in_reply_to_tweet_id")]
        public string InReplyToTweetId { get; set; }

        [JsonProperty("exclude_reply_user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ExcludeReplyUserIds { get; set; }

        public PublishTweetV2ReplyParameters()
        {
        }

        public PublishTweetV2ReplyParameters(IPublishTweetV2ReplyParameters source)
        {
            InReplyToTweetId = source.InReplyToTweetId;
            ExcludeReplyUserIds = source.ExcludeReplyUserIds;
        }
    }

    public class PublishTweetV2MediaParameter : IPublishTweetV2MediaParameter
    {
        [JsonProperty("media_ids")]
        public string[] MediaIds { get; set; }

        [JsonProperty("tagged_user_ids", NullValueHandling = NullValueHandling.Ignore)]
        public string[] TaggedUserIds { get; set; }

        public PublishTweetV2MediaParameter()
        {
        }

        public PublishTweetV2MediaParameter(IPublishTweetV2MediaParameter source)
        {
            MediaIds = source.MediaIds;
            TaggedUserIds = source.TaggedUserIds;
        }
    }

    public class PublishTweetV2Parameters : IPublishTweetV2Parameters
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <inheritdoc/>
        [JsonProperty("direct_message_deep_link", NullValueHandling = NullValueHandling.Ignore)]
        public string? DirectMessageDeepLink { get; set; }

        /// <inheritdoc/>
        [JsonProperty("for_super_followers_only", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ForSuperFollowersOnly { get; set; }

        /// <inheritdoc/>
        [JsonProperty("reply", NullValueHandling = NullValueHandling.Ignore)]
        public IPublishTweetV2ReplyParameters? Reply { get; set; }

        /// <inheritdoc/>
        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public IPublishTweetV2MediaParameter? Media { get; set; }

        /// <inheritdoc/>
        [JsonProperty("quote_tweet_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? QuoteTweetId { get; set; }

        public PublishTweetV2Parameters()
        {
        }

        public PublishTweetV2Parameters(string text)
        {
            Text = text;
        }

        public PublishTweetV2Parameters(IPublishTweetV2Parameters source)
        {
            Text = source.Text;
            DirectMessageDeepLink = source.DirectMessageDeepLink;
            ForSuperFollowersOnly = source.ForSuperFollowersOnly;
            QuoteTweetId = source.QuoteTweetId;

            if (source.Reply != null)
            {
                Reply = new PublishTweetV2ReplyParameters(source.Reply);
            }

            if (source.Media != null)
            {
                Media = new PublishTweetV2MediaParameter(source.Media);
            }
        }
    }
}
