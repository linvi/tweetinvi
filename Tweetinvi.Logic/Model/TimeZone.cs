using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.Model
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