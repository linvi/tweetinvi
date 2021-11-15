using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// A Tweet
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets</para>
    /// </summary>
    public class TimelineMetaV2
    {
        /// <summary>
        /// The Tweet ID of the oldest Tweet returned in the response.
        /// </summary>
        [JsonProperty("oldest_id")] public string OldestId { get; set; }

        /// <summary>
        /// The Tweet ID of the most recent Tweet returned in the response.
        /// </summary>
        [JsonProperty("newest_id")] public string NewestId { get; set; }

        /// <summary>
        /// The number of Tweet results returned in the response.
        /// </summary>
        [JsonProperty("count")] public int Count { get; set; }

        /// <summary>
        /// A value that encodes the next 'page' of results that can be requested,
        /// via the pagination_token request parameter.
        /// </summary>
        [JsonProperty("next_token")] public string NextToken { get; set; }

        /// <summary>
        /// A value that encodes the previous 'page' of results that can be requested,
        /// via the pagination_token request parameter.
        /// </summary>
        [JsonProperty("previous_token")] public string PreviousToken { get; set; }

    }
}
