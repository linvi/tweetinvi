using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CoordinatesV2
    {
        /// <summary>
        /// Describes the type of coordinate. The only value supported at present is Point.
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }

        /// <summary>
        /// A pair of decimal values representing the precise location of the user (latitude, longitude).
        /// This value be null unless the user explicitly shared their precise location.
        /// </summary>
        [JsonProperty("coordinates")] public int[] Coordinates { get; set; }
    }
}