using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id
    /// </summary>
    public interface IPublishTweetParameters : ICustomRequestParameters, ITweetModeParameter
    {
        /// <summary>
        /// Message to publish as a tweet
        /// </summary>
        string Text { get; set; }

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
        /// In order for a URL to not be counted in the status body of an extended Tweet, provide a URL as a Tweet attachment.
        /// This URL must be a Tweet permalink, or Direct Message deep link.
        /// Arbitrary, non-Twitter URLs must remain in the status text.
        /// URLs passed to the attachment_url parameter not matching either a Tweet permalink or
        /// Direct Message deep link will fail at Tweet creation and cause an exception.
        /// </summary>
        string QuotedTweetUrl { get; set; }

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
        /// Whether this Tweet will be published with any media attached
        /// </summary>
        bool HasMedia { get; }

        /// <summary>
        /// If you upload Tweet media that might be considered sensitive content such as
        /// nudity, violence, or medical procedures, you should set this value to true.
        /// </summary>
        bool? PossiblySensitive { get; set; }

        /// <summary>
        /// If set to true, the creator property (IUser) will only contain the id.
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

        /// <summary>
        /// Associate an ads card with the Tweet using the card_uri value from any ads card response.
        /// </summary>
        string CardUri { get; set; }
    }

    /// <inheritdoc/>
    public class PublishTweetParameters : CustomRequestParameters, IPublishTweetParameters
    {
        public PublishTweetParameters()
        {
        }

        public PublishTweetParameters(string text)
        {
            Text = text;
        }

        public PublishTweetParameters(IPublishTweetParameters source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            Text = source.Text;
            InReplyToTweet = source.InReplyToTweet;
            QuotedTweet = source.QuotedTweet;

            if (InReplyToTweet == null)
            {
                InReplyToTweetId = source.InReplyToTweetId;
            }

            MediaIds = source.MediaIds?.ToList();
            Medias = source.Medias?.ToList();
            PlaceId = source.PlaceId;
            Coordinates = source.Coordinates;
            DisplayExactCoordinates = source.DisplayExactCoordinates;
            PossiblySensitive = source.PossiblySensitive;
            TrimUser = source.TrimUser;
            AutoPopulateReplyMetadata = source.AutoPopulateReplyMetadata;
            ExcludeReplyUserIds = source.ExcludeReplyUserIds;
            TweetMode = source.TweetMode;
        }

        /// <inheritdoc/>
        public string Text { get; set; }

        /// <inheritdoc/>
        public ITweetIdentifier InReplyToTweet { get; set; }

        /// <inheritdoc/>
        public string QuotedTweetUrl { get; set; }

        /// <inheritdoc/>
        public ITweet QuotedTweet { get; set; }

        /// <inheritdoc/>
        public long? InReplyToTweetId
        {
            get => InReplyToTweet?.Id;
            set => InReplyToTweet = value != null ? new TweetIdentifier((long)value) : null;
        }

        /// <inheritdoc/>
        public List<long> MediaIds { get; set; } = new List<long>();
        /// <inheritdoc/>
        public List<IMedia> Medias { get; set; } = new List<IMedia>();
        /// <inheritdoc/>
        public bool HasMedia => MediaIds?.Count > 0 || Medias?.Count > 0;
        /// <inheritdoc/>
        public string PlaceId { get; set; }
        /// <inheritdoc/>
        public ICoordinates Coordinates { get; set; }
        /// <inheritdoc/>
        public bool? DisplayExactCoordinates { get; set; }
        /// <inheritdoc/>
        public bool? PossiblySensitive { get; set; }
        /// <inheritdoc/>
        public bool? TrimUser { get; set; }
        /// <inheritdoc/>
        public bool? AutoPopulateReplyMetadata { get; set; }
        /// <inheritdoc/>
        public IEnumerable<long> ExcludeReplyUserIds { get; set; }
        /// <inheritdoc/>
        public string CardUri { get; set; }
        /// <inheritdoc/>
        public TweetMode? TweetMode { get; set; }
    }
}