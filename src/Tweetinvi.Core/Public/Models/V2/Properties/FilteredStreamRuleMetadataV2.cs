using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamRuleMetadataV2
    {
        /// <summary>
        /// Date when the tweet was matched by the filtered stream
        /// </summary>
        [JsonProperty("sent")] public DateTimeOffset SentDate { get; set; }
    }
}