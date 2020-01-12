using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed
    /// </summary>
    public enum OEmbedTweetAlignment
    {
        None,
        Left,
        Center,
        Right
    }

    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed
    /// </summary>
    public enum OEmbedTweetTheme
    {
        Light,
        Dark
    }

    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed
    /// </summary>
    public enum OEmbedTweetWidgetType
    {
        /// <summary>
        /// Parameter won't be included
        /// </summary>
        Default,

        /// <summary>
        /// Set to video to return a Twitter Video embed for the given Tweet.
        /// </summary>
        Video
    }

    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed
    /// </summary>
    public interface IGetOEmbedTweetParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The identifier of the tweet for which you want to get the oembed tweet
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// The maximum width of a rendered Tweet in whole pixels.
        /// A supplied value under or over the allowed range will be returned as the minimum or maximum
        /// supported width respectively; the reset width value will be reflected in the returned width property.
        ///
        /// <para>Min: 220; Max: 550</para>
        /// </summary>
        int? MaxWidth { get; set; }

        /// <summary>
        /// When set to true links in a Tweet are not expanded to photo, video, or link previews.
        /// </summary>
        bool? HideMedia { get; set; }

        /// <summary>
        /// When set to true a collapsed version of the previous Tweet in a conversation thread
        /// will not be displayed when the requested Tweet is in reply to another Tweet.
        /// </summary>
        bool? HideThread { get; set; }

        /// <summary>
        /// When set to true the <script> responsible for loading widgets.js will not be returned.
        /// Your webpages should include their own reference to widgets.js
        /// for use across all Twitter widgets including Embedded Tweets.
        /// </summary>
        bool? OmitScript { get; set; }

        /// <summary>
        /// Specifies whether the embedded Tweet should be floated left, right, or center
        /// in the page relative to the parent element.
        /// </summary>
        OEmbedTweetAlignment? Alignment { get; set; }

        /// <summary>
        /// A list of Twitter usernames related to your content.
        /// This value will be forwarded to Tweet action intents if a viewer chooses
        /// to reply, like, or retweet the embedded Tweet.
        /// </summary>
        string[] RelatedUsernames { get; set; }

        /// <summary>
        /// Request returned HTML and a rendered Tweet in the specified Twitter language supported by embedded Tweets.
        /// <para> https://developer.twitter.com/en/docs/twitter-for-websites/twitter-for-websites-supported-languages/overview </para>
        /// </summary>
        Language? Language { get; set; }

        /// <summary>
        /// When set to dark, the Tweet is displayed with light text over a dark background.
        /// </summary>
        OEmbedTweetTheme? Theme { get; set; }

        /// <summary>
        /// Adjust the color of Tweet text links with a hexadecimal color value.
        /// </summary>
        string LinkColor { get; set; }

        /// <summary>
        /// Set to video to return a Twitter Video embed for the given Tweet.
        /// </summary>
        OEmbedTweetWidgetType? WidgetType { get; set; }

        /// <summary>
        /// This is the `dnt` parameter.
        /// When set to true, the Tweet and its embedded page on your site are not used for purposes
        /// that include personalized suggestions and personalized ads.
        /// </summary>
        bool? EnablePersonalisationAndSuggestions { get; set; }
    }

    /// <inheritdoc/>
    public class GetOEmbedTweetParameters : CustomRequestParameters, IGetOEmbedTweetParameters
    {
        public GetOEmbedTweetParameters(long? tweetId) : this(new TweetIdentifier(tweetId))
        {
        }

        public GetOEmbedTweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc/>
        public ITweetIdentifier Tweet { get; set; }
        /// <inheritdoc/>
        public int? MaxWidth { get; set; }
        /// <inheritdoc/>
        public bool? HideMedia { get; set; }
        /// <inheritdoc/>
        public bool? HideThread { get; set; }
        /// <inheritdoc/>
        public bool? OmitScript { get; set; }
        /// <inheritdoc/>
        public OEmbedTweetAlignment? Alignment { get; set; }
        /// <inheritdoc/>
        public string[] RelatedUsernames { get; set; }
        /// <inheritdoc/>
        public Language? Language { get; set; }
        /// <inheritdoc/>
        public OEmbedTweetTheme? Theme { get; set; }
        /// <inheritdoc/>
        public string LinkColor { get; set; }
        /// <inheritdoc/>
        public OEmbedTweetWidgetType? WidgetType { get; set; }
        /// <inheritdoc/>
        public bool? EnablePersonalisationAndSuggestions { get; set; }
    }
}