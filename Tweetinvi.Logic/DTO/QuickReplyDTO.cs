using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class QuickReplyDTO : IQuickReplyDTO
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public QuickReplyType Type { get; set; }

        [JsonProperty("options")]
        public IQuickReplyOption[] Options { get; set; }
    }
}
