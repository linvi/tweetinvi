using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetWithheldInfoV2 : WithheldInfoV2
    {
        [JsonProperty("copyright")] public bool Copyright { get; set; }
    }
}