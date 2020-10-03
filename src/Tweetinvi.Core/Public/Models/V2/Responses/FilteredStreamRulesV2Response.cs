using Newtonsoft.Json;

namespace Tweetinvi.Models.Responses
{
    public class FilteredStreamRulesV2Response
    {
        /// <summary>
        /// Filtered stream v2 filtering rules
        /// </summary>
        [JsonProperty("data")] public FilteredStreamRuleV2[] Rules { get; set; } = new FilteredStreamRuleV2[0];

        /// <summary>
        /// Metadata information about the request
        /// </summary>
        [JsonProperty("meta")] public FilteredStreamRuleMetadataV2 Meta { get; set; }

        /// <summary>
        /// All errors that prevented Twitter to send some data,
        /// but which did not prevent the request to be resolved.
        /// </summary>
        [JsonProperty("errors")] public ErrorV2[] Errors { get; set; }
    }
}