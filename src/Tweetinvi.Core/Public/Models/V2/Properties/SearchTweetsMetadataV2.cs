using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class SearchTweetsMetadataV2
    {
        /// <summary>
        /// Most recent tweet id returned in the search results
        /// </summary>
        [JsonProperty("newest_id")] public string NewestId { get; set; }

        /// <summary>
        /// Oldest tweet id returned in the search results
        /// </summary>
        [JsonProperty("oldest_id")] public string OldestId { get; set; }

        /// <summary>
        /// Number of results returned
        /// </summary>
        [JsonProperty("result_count")] public int ResultCount { get; set; }

        /// <summary>
        /// Token/cursor to access the next search page results
        /// </summary>
        [JsonProperty("next_token")] public string NextToken { get; set; }
    }
}