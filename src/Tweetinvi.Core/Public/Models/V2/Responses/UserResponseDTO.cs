using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class UserResponseDTO
    {
        [JsonProperty("data")] public UserDTO User { get; set; }
        [JsonProperty("includes")] public UserIncludesDTO Includes { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}