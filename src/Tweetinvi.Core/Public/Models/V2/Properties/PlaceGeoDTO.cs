using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PlaceGeoDTO
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("bbox")] public string[] Bbox { get; set; }
        [JsonProperty("properties")] public Dictionary<string, dynamic> Properties { get; set; }
    }
}