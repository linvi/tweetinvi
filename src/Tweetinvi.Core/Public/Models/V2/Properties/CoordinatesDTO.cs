using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CoordinatesDTO
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("coordinates")] public int[] Coordinates { get; set; }
    }
}