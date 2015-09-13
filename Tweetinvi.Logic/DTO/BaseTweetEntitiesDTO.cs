using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class BaseTweetEntitiesDTO
    {
        [JsonProperty("urls")]
        public List<IUrlEntity> Urls { get; set; }

        [JsonProperty("user_mentions")]
        public List<IUserMentionEntity> UserMentions { get; set; }

        [JsonProperty("hashtags")]
        public List<IHashtagEntity> Hashtags { get; set; }

        [JsonProperty("symbols")]
        public List<ISymbolEntity> Symbols { get; set; }
    }
}