using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.DTO
{
    public class AttachmentDTO : IAttachmentDTO
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public AttachmentType Type { get; set; }

        [JsonProperty("media")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMediaEntity Media { get; set; }
    }
}
