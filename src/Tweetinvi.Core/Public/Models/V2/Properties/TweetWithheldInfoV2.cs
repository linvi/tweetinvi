using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetWithheldInfoV2 : WithheldInfoV2
    {
        /// <summary>
        /// Indicates if the content is being withheld for on the basis of copyright infringement.
        /// </summary>
        [JsonProperty("copyright")] public bool Copyright { get; set; }
    }
}