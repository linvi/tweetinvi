using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Model;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityMessageCreatedEventDTO
    {
        [JsonProperty("direct_message_events")]
        public EventDTO[] MessageEvents { get; set; }

        [JsonProperty("apps")]
        public Dictionary<string, App> Apps { get; set; }

        [JsonProperty("users")]
        public Dictionary<string, UserDTO> UsersById { get; set; }
    }
}
