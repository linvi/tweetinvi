using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class FilteredStreamRulesV2ResponseDTO
    {
        [JsonProperty("data")] public FilteredStreamRuleDTO[] Rules { get; set; } = new FilteredStreamRuleDTO[0];
        [JsonProperty("meta")] public FilteredStreamRuleMetadataDTO Meta { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}