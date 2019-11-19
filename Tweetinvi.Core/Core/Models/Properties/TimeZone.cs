using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models.Properties
{
    public class TimeZone : ITimeZone
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tzinfo_name")]
        public string TzinfoName { get; set; }

        [JsonProperty("utc_offset")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public int UtcOffset { get; set; }
    }
}