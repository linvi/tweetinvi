using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class WithheldDTO
    {
        [JsonProperty("country_codes")] public string[] country_codes { get; set; }

        [JsonProperty("scope")] public string scope { get; set; }
    }
}