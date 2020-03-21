using Newtonsoft.Json;

namespace Tweetinvi.Models.DTO
{
    public class SearchGeoSearchResultDTO
    {
        public class GeoSearchResultDTO
        {
            [JsonProperty("places")]
            public IPlace[] Places { get; set; }
        }

        [JsonProperty("result")]
        public GeoSearchResultDTO Result { get; set; }
    }
}