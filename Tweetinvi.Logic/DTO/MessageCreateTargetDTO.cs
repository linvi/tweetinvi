using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class MessageCreateTargetDTO : IMessageCreateTargetDTO
    {
        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }
    }
}
