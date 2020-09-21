using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CoordinatesDTO
    {
        [JsonProperty("type")] public string type { get; set; }

        [JsonProperty("coordinates")] public int[] coordinates { get; set; }
    }
}