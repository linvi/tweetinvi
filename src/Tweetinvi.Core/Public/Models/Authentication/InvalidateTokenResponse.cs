using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Information about an invalidated token
    /// </summary>
    public class InvalidateTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}