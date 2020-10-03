using Newtonsoft.Json;

namespace Tweetinvi.Parameters.V2
{
    public class FilteredStreamRuleConfig
    {
        public FilteredStreamRuleConfig(string value)
        {
            Value = value;
        }

        public FilteredStreamRuleConfig(string value, string tag)
        {
            Value = value;
            Tag = tag;
        }

        [JsonProperty("value")] public string Value { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }
    }

    public interface IAddRulesToFilteredStreamV2Parameters : ICustomRequestParameters
    {
        FilteredStreamRuleConfig[] Rules { get; set; }
    }

    public class AddRulesToFilteredStreamV2Parameters : CustomRequestParameters, IAddRulesToFilteredStreamV2Parameters
    {
        public AddRulesToFilteredStreamV2Parameters(FilteredStreamRuleConfig[] rules)
        {
            Rules = rules;
        }

        public FilteredStreamRuleConfig[] Rules { get; set; }
    }
}