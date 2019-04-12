using Newtonsoft.Json;
using Tweetinvi.Core.Exceptions;

namespace Tweetinvi.Logic.Exceptions
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