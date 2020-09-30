using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamRuleMetadataDTO
    {
        [JsonProperty("sent")] public DateTimeOffset sent { get; set; }
    }
}