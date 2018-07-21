using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class EventInitiatedViaDTO : IEventInitiatedViaDTO
    {
        [JsonProperty("tweet_id")]
        public long? TweetId { get; set; }

        [JsonProperty("welcome_message_id")]
        public long? WelcomeMessageId { get; set; }
    }
}
