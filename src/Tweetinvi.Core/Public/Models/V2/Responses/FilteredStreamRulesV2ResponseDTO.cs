using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class FilteredStreamRulesV2ResponseDTO
    {
        /// <summary>
        /// Filtered stream v2 filtering rules
        /// </summary>
        [JsonProperty("data")] public FilteredStreamRuleDTO[] Rules { get; set; } = new FilteredStreamRuleDTO[0];

        /// <summary>
        /// Metadata information about the request
        /// </summary>
        [JsonProperty("meta")] public FilteredStreamRuleMetadataDTO Meta { get; set; }

        /// <summary>
        /// All errors that prevented Twitter to send some data,
        /// but which did not prevent the request to be resolved.
        /// </summary>
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}