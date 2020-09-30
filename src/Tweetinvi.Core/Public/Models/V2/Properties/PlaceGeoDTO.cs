using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PlaceGeoDTO
    {
        [JsonProperty("type")] public string type { get; set; }

        [JsonProperty("bbox")] public string[] bbox { get; set; }

        [JsonProperty("properties")] public Dictionary<string, dynamic> properties { get; set; }
    }
}