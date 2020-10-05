using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserDescriptionEntitiesV2
    {
        /// <summary>
        /// Contains details about text recognized as a Cashtag.
        /// </summary>
        [JsonProperty("cashtags")] public CashtagV2[] Cashtags { get; set; }

        /// <summary>
        /// Contains details about text recognized as a Hashtag.
        /// </summary>
        [JsonProperty("hashtags")] public HashtagV2[] Hashtags { get; set; }

        /// <summary>
        /// Contains details about text recognized as a user mention.
        /// </summary>
        [JsonProperty("mentions")] public UserMentionV2[] Mentions { get; set; }

        /// <summary>
        /// Contains details about any URLs included in the user's description.
        /// </summary>
        [JsonProperty("urls")] public UrlV2[] Urls { get; set; }
    }
}