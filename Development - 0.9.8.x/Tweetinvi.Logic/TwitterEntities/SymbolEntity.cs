using Newtonsoft.Json;
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

            if (Indices.Length != other.Indices.Length)
            {
                return false;
            }

            for (int i = 0; i < Indices.Length; ++i)
            {
                if (Indices[i] != other.Indices[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}