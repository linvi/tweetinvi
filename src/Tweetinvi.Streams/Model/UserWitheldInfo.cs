using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Streams.Model
{
    public class UserWitheldInfo : IUserWitheldInfo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("withheld_in_countries")]
        public IEnumerable<string> WitheldInCountries { get; set; }
    }
}