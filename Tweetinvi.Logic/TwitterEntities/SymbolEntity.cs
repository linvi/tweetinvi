using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class SymbolEntity : ISymbolEntity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        public bool Equals(ISymbolEntity other)
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