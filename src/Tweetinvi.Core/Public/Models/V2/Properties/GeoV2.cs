using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class GeoV2
    {
        [JsonProperty("coordinates")] public CoordinatesV2 Coordinates { get; set; }
        [JsonProperty("place_id")] public string PlaceId { get; set; }
    }
}