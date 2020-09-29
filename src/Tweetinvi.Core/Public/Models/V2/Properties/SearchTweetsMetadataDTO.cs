using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class SearchTweetsMetadataDTO
    {
        [JsonProperty("newest_id")] public string newest_id { get; set; }
        [JsonProperty("oldest_id")] public string oldest_id { get; set; }
        [JsonProperty("result_count")] public int result_count { get; set; }
        [JsonProperty("next_token")] public string next_token { get; set; }
    }
}