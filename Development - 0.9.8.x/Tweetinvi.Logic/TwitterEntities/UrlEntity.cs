using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    /// <summary>
    /// Object storing information related with an URL on twitter
    /// </summary>
    public class UrlEntity : IUrlEntity
    {
        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("display_url")]
        public string DisplayedURL { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedURL { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        public bool Equals(IUrlEntity other)
        {
            if (URL != other.URL ||
                ExpandedURL != other.ExpandedURL ||
                DisplayedURL != other.DisplayedURL)
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