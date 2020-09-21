using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PlaceDTO
    {
        [JsonProperty("contained_within")] public string[] contained_within { get; set; }

        [JsonProperty("country")] public string country { get; set; }

        [JsonProperty("country_code")] public string country_code { get; set; }

        [JsonProperty("full_name")] public string full_name { get; set; }

        [JsonProperty("geo")] public PlaceGeoDTO geo { get; set; }

        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("name")] public string name { get; set; }

        [JsonProperty("place_type")] public string place_type { get; set; }

    }
}