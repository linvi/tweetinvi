using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class UserResponseDTO
    {
        [JsonProperty("data")] public UserDTO data { get; set; }

        [JsonProperty("includes")] public UserIncludesDTO includes { get; set; }

        [JsonProperty("errors")] public ErrorDTO[] errors { get; set; }
    }
}