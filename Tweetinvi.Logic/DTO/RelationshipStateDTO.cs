using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class RelationshipStateDTO : IRelationshipStateDTO
    {
        private List<string> _connections;

        [JsonProperty("id")]
        public long TargetUserId { get; set; }

        [JsonProperty("id_str")]
        public string TargetUserIdStr { get; set; }

        [JsonProperty("name")]
        public string TargetUserName { get; set; }

        [JsonProperty("screen_name")]
        public string TargetUserScreenName { get; set; }

        [JsonIgnore]
        public bool Following { get; set; }
        
        [JsonIgnore]
        public bool FollowedBy { get; set; }

        [JsonIgnore]
        public bool FollowingRequested { get; set; }

        [JsonIgnore]
        public bool FollowingRequestReceived { get; set; }

        [JsonProperty("connections")]
        public List<string> Connections
        {
            get { return _connections; }
            set
            {
                _connections = value;
                UpdateConnectionInfos();
            }
        }
        
        private void UpdateConnectionInfos()
        {
            Following = _connections.Contains("following");
            FollowedBy = _connections.Contains("followed_by");
            FollowingRequested = _connections.Contains("following_requested");
            FollowingRequestReceived = _connections.Contains("following_received");
        }
    }
}