using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class UsersResponseDTO
    {
        [JsonProperty("data")] public UserDTO[] Users { get; set; }
        [JsonProperty("includes")] public UserIncludesDTO Includes { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}