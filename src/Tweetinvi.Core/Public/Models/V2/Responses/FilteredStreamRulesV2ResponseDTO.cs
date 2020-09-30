using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;

namespace Tweetinvi.Models.V2.Responses
{
    public class FilteredStreamDeleteOperation
    {
        public FilteredStreamDeleteOperation(string[] ids)
        {
            this.ids = ids;
        }
        [JsonProperty("ids")] public string[] ids { get; set; }
    }

    public class FilteredStreamOperations
    {
        [JsonProperty("add")] public FilteredStreamRuleContentDTO[] add { get; set; }
        [JsonProperty("delete")] public FilteredStreamDeleteOperation delete { get; set; }
    }

    public class FilteredStreamRuleContentDTO
    {
        [JsonProperty("value")] public string value { get; set; }
        [JsonProperty("tag")] public string tag { get; set; }
    }

    public class FilteredStreamRuleDTO : FilteredStreamRuleContentDTO
    {
        [JsonProperty("id")] public string id { get; set; }

    }

    public class FilteredStreamRuleMetadataDTO
    {
        [JsonProperty("sent")] public DateTimeOffset sent { get; set; }
    }

    public class FilteredStreamRulesV2ResponseDTO
    {
        [JsonProperty("data")] public FilteredStreamRuleDTO[] data { get; set; } = new FilteredStreamRuleDTO[0];
        [JsonProperty("meta")] public FilteredStreamRuleMetadataDTO meta { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] errors { get; set; }
    }
}