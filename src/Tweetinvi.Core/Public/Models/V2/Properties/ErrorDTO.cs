using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class ErrorDTO
    {
        // client errors
        [JsonProperty("client_id")] public string client_id { get; set; }
        [JsonProperty("required_enrollment")] public string required_enrollment { get; set; }
        [JsonProperty("registration_url")] public string registration_url { get; set; }
        [JsonProperty("title")] public string title { get; set; }
        [JsonProperty("detail")] public string detail { get; set; }
        [JsonProperty("reason")] public string reason { get; set; }
        [JsonProperty("type")] public string type { get; set; }

        // parameters error
        [JsonProperty("resource_type")] public string resource_type { get; set; }
        [JsonProperty("parameter")] public string parameter { get; set; }
        [JsonProperty("value")] public string value { get; set; }
    }
}