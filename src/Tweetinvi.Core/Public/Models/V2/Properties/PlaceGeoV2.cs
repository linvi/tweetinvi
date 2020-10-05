using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PlaceGeoV2
    {
        /// <summary>
        /// Type of geometry used
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }

        /// <summary>
        /// Bounding box array containing the place coordinates
        /// </summary>
        [JsonProperty("bbox")] public double[] Bbox { get; set; }

        /// <summary>
        /// Special information about this place
        /// </summary>
        [JsonProperty("properties")] public Dictionary<string, dynamic> Properties { get; set; }
    }
}