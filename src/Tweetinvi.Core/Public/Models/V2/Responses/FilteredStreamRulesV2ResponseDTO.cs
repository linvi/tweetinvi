using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class FilteredStreamRulesV2ResponseDTO
    {
        [JsonProperty("data")] public FilteredStreamRuleDTO[] data { get; set; } = new FilteredStreamRuleDTO[0];
        [JsonProperty("meta")] public FilteredStreamRuleMetadataDTO meta { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] errors { get; set; }
    }
}