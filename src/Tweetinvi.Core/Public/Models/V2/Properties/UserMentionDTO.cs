using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserMentionDTO
    {
        [JsonProperty("end")] public int end { get; set; }

        [JsonProperty("start")] public int start { get; set; }

        [JsonProperty("username")] public string username { get; set; }
    }
}