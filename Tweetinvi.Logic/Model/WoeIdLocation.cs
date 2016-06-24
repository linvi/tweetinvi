using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class WoeIdLocation : IWoeIdLocation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("woeid")]
        public long WoeId { get; set; }
    }
}