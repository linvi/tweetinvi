using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class SearchMetadata : ISearchMetadata
    {
        [JsonProperty("completed_in")]
        public double CompletedIn { get; set; }

        [JsonProperty("max_id")]
        public long MaxId { get; private set; }

        [JsonProperty("max_id_str")]
        public string MaxIdStr { get; private set; }

        [JsonProperty("next_results")]
        public string NextResults { get; private set; }

        [JsonProperty("query")]
        public string Query { get; private set; }

        [JsonProperty("refresh_url")]
        public string RefreshURL { get; private set; }

        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("since_id")]
        public long SinceId { get; private set; }

        [JsonProperty("since_id_str")]
        public string SinceIdStr { get; private set; }
    }
}
