using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class QuickReplyResponse : IQuickReplyResponse
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public QuickReplyType Type { get; }

        [JsonProperty("metadata")]
        public string Metadata { get; }
    }
}
