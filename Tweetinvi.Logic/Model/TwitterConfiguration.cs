using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.Model
{
    public class TwitterConfiguration : ITwitterConfiguration
    {
        [JsonProperty("characters_reserved_per_media")]
        public int CharactersReservedPerMedia { get; private set; }

        [JsonProperty("dm_text_character_limit")]
        public int MessageTextCharacterLimit { get; private set; }

        [JsonProperty("max_media_per_upload")]
        public int MaxMediaPerUpload { get; private set; }

        [JsonProperty("non_username_paths")]
        public string[] NonUsernamePaths { get; private set; }

        [JsonProperty("photo_size_limit")]
        public int PhotoSizeLimit { get; private set; }

        [JsonProperty("photo_sizes")]
        public Dictionary<string, IMediaEntitySize> PhotoSizes { get; private set; }

        [JsonProperty("short_url_length")]
        public int ShortURLLength { get; private set; }

        [JsonProperty("short_url_length_https")]
        public int ShortURLLengthHttps { get; private set; }
    }
}