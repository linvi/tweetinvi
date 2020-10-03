using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class PlaceV2
    {
        [JsonProperty("contained_within")] public string[] ContainedWithin { get; set; }
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("country_code")] public string CountryCode { get; set; }
        [JsonProperty("full_name")] public string FullName { get; set; }
        [JsonProperty("geo")] public PlaceGeoV2 PlaceGeo { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("place_type")] public string PlaceType { get; set; }
    }
}