using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class UserEntities : IUserEntities
    {
        [JsonProperty("url")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IWebsiteEntity Website { get; set; }

        [JsonProperty("description")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IDescriptionEntity Description { get; set; }
    }
}