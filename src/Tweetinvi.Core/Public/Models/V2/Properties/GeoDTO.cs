using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class GeoDTO
    {
        [JsonProperty("coordinates")] public CoordinatesDTO coordinates { get; set; }

        [JsonProperty("place_id")] public string place_id { get; set; }
    }
}