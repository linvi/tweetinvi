using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class UserEntities : IUserEntities
    {
        [JsonProperty("url")]
        public IWebsiteEntity Website { get; set; }

        [JsonProperty("description")]
        public IDescriptionEntity Description { get; set; }
    }
}