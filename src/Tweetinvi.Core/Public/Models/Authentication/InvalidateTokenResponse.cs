using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class InvalidateTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}