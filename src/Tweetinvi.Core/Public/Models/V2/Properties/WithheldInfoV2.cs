using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class WithheldInfoV2
    {
        [JsonProperty("country_codes")] public string[] CountryCodes { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }
    }
}