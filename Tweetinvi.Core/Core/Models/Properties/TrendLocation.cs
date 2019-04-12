using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class TrendLocation : ITrendLocation
    {
        private class PlaceTypeDTO
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("code")]
            public int Code { get; set; }
        }

        [JsonProperty("woeid")]
        public long WoeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("parentid")]
        public long ParentId { get; set; }

        [JsonProperty]
        public PlaceType PlaceType
        {
            get
            {
                if (_placeTypeDTO == null)
                {
                    return PlaceType.Undefined;
                }

                return (PlaceType)_placeTypeDTO.Code;
            }
            set
            {
                if (_placeTypeDTO == null)
                {
                    _placeTypeDTO = new PlaceTypeDTO();
                }

                _placeTypeDTO.Code = (int) value;
            }
        }

        [JsonProperty("placeType")]
        private PlaceTypeDTO _placeTypeDTO { get; set; }
    }
}