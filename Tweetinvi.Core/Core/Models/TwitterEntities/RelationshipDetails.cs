using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.TwitterEntities
{
    public class RelationshipDetails : IRelationshipDetails
    {
        public IRelationshipDetailsDTO RelationshipDetailsDTO { get; set; }

        public RelationshipDetails(IRelationshipDetailsDTO relationshipDetailsDTO)
        {
            RelationshipDetailsDTO = relationshipDetailsDTO;
        }

        public long SourceId
        {
            get { return RelationshipDetailsDTO.SourceId; }
        }

        public string SourceIdStr
        {
            get { return RelationshipDetailsDTO.SourceIdStr; }
        }

        public string SourceScreenName
        {
            get { return RelationshipDetailsDTO.SourceScreenName; }
        }

        public long TargetId
        {
            get { return RelationshipDetailsDTO.TargetId; }
        }

        public string TargetIdStr
        {
            get { return RelationshipDetailsDTO.TargetIdStr; }
        }

        public string TargetScreenName
        {
            get { return RelationshipDetailsDTO.TargetScreenName; }
        }

        public bool Following
        {
            get { return RelationshipDetailsDTO.Following; }
        }

        public bool FollowedBy
        {
            get { return RelationshipDetailsDTO.FollowedBy; }
        }

        public bool FollowingRequestReceived
        {
            get { return RelationshipDetailsDTO.FollowingReceived; }
        }

        public bool FollowingRequested
        {
            get { return RelationshipDetailsDTO.FollowingRequested; }
        }

        public bool NotificationsEnabled
        {
            get { return RelationshipDetailsDTO.NotificationsEnabled; }
        }

        public bool CanSendDirectMessage
        {
            get { return RelationshipDetailsDTO.CanSendDirectMessage; }
        }

        public bool Blocking
        {
            get { return RelationshipDetailsDTO.Blocking; }
        }

        public bool BlockedBy
        {
            get { return RelationshipDetailsDTO.BlockedBy; }
        }

        public bool Muting
        {
            get { return RelationshipDetailsDTO.Muting; }
        }

        public bool WantRetweets
        {
            get { return RelationshipDetailsDTO.WantRetweets; }
        }

        public bool AllReplies
        {
            get { return RelationshipDetailsDTO.AllReplies; }
        }

        public bool MarkedSpam
        {
            get { return RelationshipDetailsDTO.MarkedSpam; }
        }
    }
}