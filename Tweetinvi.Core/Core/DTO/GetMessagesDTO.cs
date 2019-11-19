using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Logic.DTO
{
    public class GetMessagesDTO : IGetMessagesDTO
    {
        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }

        [JsonProperty("events")]
        public IMessageEventDTO[] MessageEvents { get; set; }

        [JsonProperty("apps")]
        public Dictionary<long, IApp> Apps { get; set; }
    }
}
