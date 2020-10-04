using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// The place tagged in a Tweet
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/place </para>
    /// </summary>
    public class PlaceV2
    {
        /// <summary>
        /// Returns the identifiers of known places that contain the referenced place.
        /// </summary>
        [JsonProperty("contained_within")] public string[] ContainedWithin { get; set; }

        /// <summary>
        /// The full-length name of the country this place belongs to.
        /// </summary>
        [JsonProperty("country")] public string Country { get; set; }

        /// <summary>
        /// The ISO Alpha-2 country code this place belongs to.
        /// </summary>
        [JsonProperty("country_code")] public string CountryCode { get; set; }

        /// <summary>
        /// A longer-form detailed place name.
        /// </summary>
        [JsonProperty("full_name")] public string FullName { get; set; }

        /// <summary>
        /// Contains place details in GeoJSON format.
        /// </summary>
        [JsonProperty("geo")] public PlaceGeoV2 PlaceGeo { get; set; }

        /// <summary>
        /// The unique identifier of the expanded place, if this is a point of interest tagged in the Tweet.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// The short name of this place
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }

        /// <summary>
        /// Specified the particular type of information represented by this place information, such as a city name, or a point of interest.
        /// </summary>
        [JsonProperty("place_type")] public string PlaceType { get; set; }
    }
}