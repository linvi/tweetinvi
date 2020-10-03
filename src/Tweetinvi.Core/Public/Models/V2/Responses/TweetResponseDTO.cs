using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetResponseDTO
    {
        /// <summary>
        /// Tweet returned by the request
        /// </summary>
        [JsonProperty("data")] public TweetDTO Tweet { get; set; }

        /// <summary>
        /// Contains all the requested expansions
        /// </summary>
        [JsonProperty("includes")] public TweetIncludesDTO Includes { get; set; }

        /// <summary>
        /// All errors that prevented Twitter to send some data,
        /// but which did not prevent the request to be resolved.
        /// </summary>
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}