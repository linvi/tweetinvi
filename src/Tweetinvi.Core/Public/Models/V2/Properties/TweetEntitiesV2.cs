using Newtonsoft.Json;

using Tweetinvi.Core.Models.TwitterEntities.ExtendedEntities;

namespace Tweetinvi.Models.V2
{
    public class TweetEntitiesV2
    {
        /// <summary>
        /// Entities are comprised of people, places, products, and organizations.
        /// Entities are delivered as part of the entity payload section.
        /// They are programmatically assigned based on what is explicitly mentioned in the Tweet text.
        /// <para>Read more: https://developer.twitter.com/en/docs/twitter-api/annotations </para>
        /// </summary>
        [JsonProperty("annotations")] public TweetAnnotationV2[] Annotations { get; set; }

        /// <summary>
        /// Cashtags found in the tweet. A cashtag is a company ticker symbol preceded by the U.S. dollar sign, e.g. $TWTR.
        /// </summary>
        [JsonProperty("cashtags")] public CashtagV2[] Cashtags { get; set; }

        /// <summary>
        /// Hashtags found in the tweet.
        /// </summary>
        [JsonProperty("hashtags")] public HashtagV2[] Hashtags { get; set; }

        /// <summary>
        /// Mentions found in the tweet.
        /// </summary>
        [JsonProperty("mentions")] public UserMentionV2[] Mentions { get; set; }

        /// <summary>
        /// Urls found in the tweet.
        /// </summary>
        [JsonProperty("urls")] public UrlV2[] Urls { get; set; }

        [JsonIgnore] public MediaV2[] Medias { get; set; }
    }
}