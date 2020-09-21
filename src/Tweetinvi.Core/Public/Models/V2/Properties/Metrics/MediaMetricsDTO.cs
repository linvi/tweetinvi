using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class MediaMetricsDTO
    {
        [JsonProperty("playback_0_count")] public int playback_0_count { get; set; }

        [JsonProperty("playback_25_count")] public int playback_25_count { get; set; }

        [JsonProperty("playback_50_count")] public int playback_50_count { get; set; }

        [JsonProperty("playback_75_count")] public int playback_75_count { get; set; }

        [JsonProperty("playback_100_count")] public int playback_100_count { get; set; }
    }
}