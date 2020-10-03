using Newtonsoft.Json;

namespace Tweetinvi.Models.Responses
{
    public class UsersV2Response
    {
        /// <summary>
        /// Users returned by the request
        /// </summary>
        [JsonProperty("data")] public UserV2[] Users { get; set; }

        /// <summary>
        /// Contains all the requested expansions
        /// </summary>
        [JsonProperty("includes")] public UserIncludesV2 Includes { get; set; }

        /// <summary>
        /// All errors that prevented Twitter to send some data,
        /// but which did not prevent the request to be resolved.
        /// </summary>
        [JsonProperty("errors")] public ErrorV2[] Errors { get; set; }
    }
}