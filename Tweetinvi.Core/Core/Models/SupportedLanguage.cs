using Newtonsoft.Json;

namespace Tweetinvi.Core.Models
{
    public class SupportedLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}