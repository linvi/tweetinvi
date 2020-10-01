using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserMentionDTO
    {
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
    }
}