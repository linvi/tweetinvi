using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// Class storing multiple types of TweetEntities
    /// https://dev.twitter.com/docs/tweet-entities
    /// </summary>
    public class TweetEntitiesDTO : BaseTweetEntitiesDTO, ITweetEntities
    {
        [JsonProperty("media")]
        public List<IMediaEntity> Medias { get; set; }
    }
}