using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class FilteredStreamRuleMetadataV2
    {
        [JsonProperty("sent")] public DateTimeOffset SentDate { get; set; }
    }
}