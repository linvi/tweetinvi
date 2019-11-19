using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Model;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class BaseAccountActivityMessageEventDTO
    {
        [JsonProperty("apps")]
        public Dictionary<string, App> Apps { get; set; }

        [JsonProperty("users")]
        public Dictionary<string, UserDTO> UsersById { get; set; }
    }
}
