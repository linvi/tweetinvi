using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// A hashtag is a keyword prefixed by # and representing a category of tweet
    /// This class stores information related with an user mention
    /// </summary>
    public class HashtagEntity : IHashtagEntity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        public bool Equals(IHashtagEntity other)
        {
            if (Text != other.Text)
            {
                return false;
            }

            if (Indices == null || other.Indices == null)
            {
                return Indices == other.Indices;
            }

            return Indices.ContainsSameObjectsAs(other.Indices, true);
        }
    }
}