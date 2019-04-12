using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class RelationshipState : IRelationshipState
    {
        public IRelationshipStateDTO RelationshipStateDTO { get; set; }

        public RelationshipState(IRelationshipStateDTO relationshipStateDTO)
        {
            RelationshipStateDTO = relationshipStateDTO;
        }

        public long TargetId
        {
            get { return RelationshipStateDTO.TargetUserId; }
        }

        public string TargetIdStr
        {
            get { return RelationshipStateDTO.TargetUserIdStr; }
        }

        public string TargetName
        {
            get { return RelationshipStateDTO.TargetUserName; }
        }

        public string TargetScreenName
        {
            get { return RelationshipStateDTO.TargetUserScreenName; }
        }

        public bool Following
        {
            get { return RelationshipStateDTO.Following; }
        }

        public bool FollowedBy
        {
            get { return RelationshipStateDTO.FollowedBy; }
        }

        public bool FollowingRequested
        {
            get { return RelationshipStateDTO.FollowingRequested; }
        }

        public bool FollowingRequestReceived
        {
            get { return RelationshipStateDTO.FollowingRequestReceived; }
        }
    }
}