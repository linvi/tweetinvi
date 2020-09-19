using System;
using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models
{
    public class GetTrendsAtResult : IGetTrendsAtResult
    {
        [JsonProperty("as_of")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public DateTimeOffset AsOf { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("locations")]
        public IWoeIdLocation[] WoeIdLocations { get; set; }

        [JsonProperty("trends")]
        public ITrend[] Trends { get; set; }
    }
}