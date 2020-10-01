using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class MediaMetricsDTO
    {
        [JsonProperty("playback_0_count")] public int Playback0Count { get; set; }
        [JsonProperty("playback_25_count")] public int Playback25Count { get; set; }
        [JsonProperty("playback_50_count")] public int Playback50Count { get; set; }
        [JsonProperty("playback_75_count")] public int Playback75Count { get; set; }
        [JsonProperty("playback_100_count")] public int Playback100Count { get; set; }
    }
}