using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models.Properties
{
    public class App : IApp
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
