using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class UserResponseDTO
    {
        /// <summary>
        /// User returned by the request
        /// </summary>
        [JsonProperty("data")] public UserDTO User { get; set; }

        /// <summary>
        /// Contains all the requested expansions
        /// </summary>
        [JsonProperty("includes")] public UserIncludesDTO Includes { get; set; }

        /// <summary>
        /// All errors that prevented Twitter to send some data,
        /// but which did not prevent the request to be resolved.
        /// </summary>
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}