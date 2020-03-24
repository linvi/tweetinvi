using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{
    public class RelationshipDetailsDTO : IRelationshipDetailsDTO
    {
        [JsonProperty("relationship")]
        private TwitterRelationshipDTO _relationship { get; set; }

        private SourceRelationshipDTO _sourceAccessor => _relationship.Source;

        private TargetRelationshipDTO _targetAccessor => _relationship.Target;

        public long SourceId => _sourceAccessor.SourceId;

        public string SourceIdStr => _sourceAccessor.SourceIdStr;

        public string SourceScreenName => _sourceAccessor.SourceScreenName;

        public long TargetId => _targetAccessor.TargetId;

        public string TargetIdStr => _targetAccessor.TargetIdStr;

        public string TargetScreenName => _targetAccessor.TargetScreenName;

        [JsonIgnore]
        public bool Following => _sourceAccessor.Following;

        [JsonIgnore]
        public bool FollowedBy => _sourceAccessor.FollowedBy;

        [JsonIgnore]
        public bool FollowingReceived => _sourceAccessor.FollowingReceived;

        [JsonIgnore]
        public bool FollowingRequested => _sourceAccessor.FollowingRequested;

        [JsonIgnore]
        public bool NotificationsEnabled => _sourceAccessor.NotificationsEnabled;

        [JsonIgnore]
        public bool CanSendDirectMessage => _sourceAccessor.CanSendDirectMessage;

        [JsonIgnore]
        public bool Blocking => _sourceAccessor.Blocking;

        [JsonIgnore]
        public bool BlockedBy => _sourceAccessor.BlockedBy;

        [JsonIgnore]
        public bool Muting => _sourceAccessor.Muting;

        [JsonIgnore]
        public bool WantRetweets => _sourceAccessor.WantRetweets;

        [JsonIgnore]
        public bool AllReplies => _sourceAccessor.AllReplies;

        [JsonIgnore]
        public bool MarkedSpam => _sourceAccessor.MarkedSpam;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable ClassNeverInstantiated.Local
        private class TwitterRelationshipDTO
        {
            [JsonProperty("source")]
            public SourceRelationshipDTO Source { get; set; }

            [JsonProperty("target")]
            public TargetRelationshipDTO Target { get; set; }
        }

        private class SourceRelationshipDTO
        {
            [JsonProperty("id")]
            public long SourceId { get; private set; }

            [JsonProperty("id_str")]
            public string SourceIdStr { get; private set; }

            [JsonProperty("screen_name")]
            public string SourceScreenName { get; private set; }

            [JsonProperty("following")]
            public bool Following { get; set; }

            [JsonProperty("followed_by")]
            public bool FollowedBy { get; set; }

            [JsonProperty("following_received")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool FollowingReceived { get; set; }

            [JsonProperty("following_requested")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool FollowingRequested { get; set; }

            [JsonProperty("notifications_enabled")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool NotificationsEnabled { get; set; }

            [JsonProperty("can_dm")]
            public bool CanSendDirectMessage { get; set; }

            [JsonProperty("blocking")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool Blocking { get; set; }

            [JsonProperty("blocked_by")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool BlockedBy { get; set; }

            [JsonProperty("muting")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool Muting { get; set; }

            [JsonProperty("want_retweets")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool WantRetweets { get; set; }

            [JsonProperty("all_replies")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool AllReplies { get; set; }

            [JsonProperty("marked_spam")]
            [JsonConverter(typeof(JsonPropertyConverterRepository))]
            public bool MarkedSpam { get; set; }
        }

        private class TargetRelationshipDTO
        {
            [JsonProperty("id")]
            public long TargetId { get; set; }

            [JsonProperty("id_str")]
            public string TargetIdStr { get; set; }

            [JsonProperty("screen_name")]
            public string TargetScreenName { get; set; }
        }
        // ReSharper restore ClassNeverInstantiated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}