using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamRuleMetadataV2
    {
        [JsonProperty("sent")] public DateTimeOffset SentDate { get; set; }
    }
}