using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class AttachmentsDTO
    {
        [JsonProperty("media_keys")] public string[] media_keys { get; set; }

        [JsonProperty("poll_ids")] public string[] poll_ids { get; set; }
    }
}