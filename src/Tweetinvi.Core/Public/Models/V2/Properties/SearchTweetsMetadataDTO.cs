using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class SearchTweetsMetadataDTO
    {
        [JsonProperty("newest_id")] public string NewestId { get; set; }
        [JsonProperty("oldest_id")] public string OldestId { get; set; }
        [JsonProperty("result_count")] public int ResultCount { get; set; }
        [JsonProperty("next_token")] public string NextToken { get; set; }
    }
}