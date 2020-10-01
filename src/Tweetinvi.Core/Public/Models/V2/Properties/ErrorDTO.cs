using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class ErrorDTO
    {
        // client errors
        [JsonProperty("client_id")] public string ClientId { get; set; }
        [JsonProperty("required_enrollment")] public string RequiredEnrollment { get; set; }
        [JsonProperty("registration_url")] public string RegistrationUrl { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("detail")] public string Detail { get; set; }
        [JsonProperty("reason")] public string Reason { get; set; }
        [JsonProperty("type")] public string Type { get; set; }

        // parameters error
        [JsonProperty("resource_type")] public string ResourceType { get; set; }
        [JsonProperty("parameter")] public string Parameter { get; set; }
        [JsonProperty("value")] public string Value { get; set; }
    }
}