using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class GeoDTO
    {
        [JsonProperty("coordinates")] public CoordinatesDTO Coordinates { get; set; }
        [JsonProperty("place_id")] public string PlaceId { get; set; }
    }
}