using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Models.TwitterEntities
{
    public class RelationshipDetails : IRelationshipDetails
    {
        public IRelationshipDetailsDTO RelationshipDetailsDTO { get; set; }

        public RelationshipDetails(IRelationshipDetailsDTO relationshipDetailsDTO)
        {
            RelationshipDetailsDTO = relationshipDetailsDTO;
        }

        public long SourceId => RelationshipDetailsDTO.SourceId;

        public string SourceIdStr => RelationshipDetailsDTO.SourceIdStr;

        public string SourceScreenName => RelationshipDetailsDTO.SourceScreenName;

        public long TargetId => RelationshipDetailsDTO.TargetId;

        public string TargetIdStr => RelationshipDetailsDTO.TargetIdStr;

        public string TargetScreenName => RelationshipDetailsDTO.TargetScreenName;

        public bool Following => RelationshipDetailsDTO.Following;

        public bool FollowedBy => RelationshipDetailsDTO.FollowedBy;

        public bool FollowingRequestReceived => RelationshipDetailsDTO.FollowingReceived;

        public bool FollowingRequested => RelationshipDetailsDTO.FollowingRequested;

        public bool NotificationsEnabled => RelationshipDetailsDTO.NotificationsEnabled;

        public bool CanSendDirectMessage => RelationshipDetailsDTO.CanSendDirectMessage;

        public bool Blocking => RelationshipDetailsDTO.Blocking;

        public bool BlockedBy => RelationshipDetailsDTO.BlockedBy;

        public bool Muting => RelationshipDetailsDTO.Muting;

        public bool WantRetweets => RelationshipDetailsDTO.WantRetweets;

        public bool AllReplies => RelationshipDetailsDTO.AllReplies;

        public bool MarkedSpam => RelationshipDetailsDTO.MarkedSpam;
    }
}