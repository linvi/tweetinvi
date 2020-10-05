using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class GeoV2
    {
        /// <summary>
        /// A pair of decimal values representing the precise location of the user (latitude, longitude).
        /// This value be null unless the user explicitly shared their precise location.
        /// </summary>
        [JsonProperty("coordinates")] public CoordinatesV2 Coordinates { get; set; }

        /// <summary>
        /// The unique identifier of the place, if this is a point of interest tagged in the Tweet.
        /// </summary>
        [JsonProperty("place_id")] public string PlaceId { get; set; }
    }
}