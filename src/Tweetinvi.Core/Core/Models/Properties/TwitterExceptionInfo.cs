using Newtonsoft.Json;
using Tweetinvi.Core.Exceptions;

namespace Tweetinvi.Core.Models.Properties
{
    public class TwitterExceptionInfo : ITwitterExceptionInfo
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}