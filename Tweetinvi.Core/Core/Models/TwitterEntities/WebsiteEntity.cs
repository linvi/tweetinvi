using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Core.Models.TwitterEntities
{
    public class WebsiteEntity : IWebsiteEntity
    {
        [JsonProperty("urls")]
        public IEnumerable<IUrlEntity> Urls { get; set; }
    }
}