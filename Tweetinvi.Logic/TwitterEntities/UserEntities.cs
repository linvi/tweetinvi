using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.JsonConverters;

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