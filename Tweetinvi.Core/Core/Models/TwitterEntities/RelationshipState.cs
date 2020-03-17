using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Models.TwitterEntities
{
    public class RelationshipState : IRelationshipState
    {
        public IRelationshipStateDTO RelationshipStateDTO { get; set; }

        public RelationshipState(IRelationshipStateDTO relationshipStateDTO)
        {
            RelationshipStateDTO = relationshipStateDTO;
        }

        public long TargetId => RelationshipStateDTO.TargetUserId;

        public string TargetIdStr => RelationshipStateDTO.TargetUserIdStr;

        public string TargetName => RelationshipStateDTO.TargetUserName;

        public string TargetScreenName => RelationshipStateDTO.TargetUserScreenName;

        public bool Following => RelationshipStateDTO.Following;

        public bool FollowedBy => RelationshipStateDTO.FollowedBy;

        public bool FollowingRequested => RelationshipStateDTO.FollowingRequested;

        public bool FollowingRequestReceived => RelationshipStateDTO.FollowingRequestReceived;
    }
}