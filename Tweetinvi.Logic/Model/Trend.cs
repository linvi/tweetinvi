using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Logic.Model
{
    public class Trend : ITrend
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("promoted_content")]
        public string PromotedContent { get; set; }

        [JsonProperty("tweet_volume")] 
        public int? TweetVolume { get; set; }
    }
}