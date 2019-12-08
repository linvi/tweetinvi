using Newtonsoft.Json;

namespace Tweetinvi.Core.DTO
{
    public class CreateTokenResponseDTO
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}