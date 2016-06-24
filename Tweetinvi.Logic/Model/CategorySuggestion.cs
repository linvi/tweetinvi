using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class CategorySuggestion : ICategorySuggestion
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("slug")]
        public string Slug { get; private set; }

        [JsonProperty("size")]
        public int Size { get; private set; }
    }
}
