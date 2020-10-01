using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PlaceDTO
    {
        [JsonProperty("contained_within")] public string[] ContainedWithin { get; set; }
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("country_code")] public string CountryCode { get; set; }
        [JsonProperty("full_name")] public string FullName { get; set; }
        [JsonProperty("geo")] public PlaceGeoDTO PlaceGeo { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("place_type")] public string PlaceType { get; set; }
    }
}