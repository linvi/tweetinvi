using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class QuickReplyOption : IQuickReplyOption
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
