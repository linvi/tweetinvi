using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class RelationshipState : IRelationshipState
    {
        private readonly IRelationshipStateDTO _relationshipStateDTO;

        public RelationshipState(IRelationshipStateDTO relationshipStateDTO)
        {
            _relationshipStateDTO = relationshipStateDTO;
        }

        public long TargetId
        {
            get { return _relationshipStateDTO.TargetUserId; }
        }

        public string TargetIdStr
        {
            get { return _relationshipStateDTO.TargetUserIdStr; }
        }

        public string TargetName
        {
            get { return _relationshipStateDTO.TargetUserName; }
        }

        public string TargetScreenName
        {
            get { return _relationshipStateDTO.TargetUserScreenName; }
        }

        public bool Following
        {
            get { return _relationshipStateDTO.Following; }
        }

        public bool FollowedBy
        {
            get { return _relationshipStateDTO.FollowedBy; }
        }

        public bool FollowingRequested
        {
            get { return _relationshipStateDTO.FollowingRequested; }
        }

        public bool FollowingRequestReceived
        {
            get { return _relationshipStateDTO.FollowingRequestReceived; }
        }
    }
}