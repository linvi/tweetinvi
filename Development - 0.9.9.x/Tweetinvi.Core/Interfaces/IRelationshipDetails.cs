namespace Tweetinvi.Core.Interfaces
{
    public interface IRelationshipDetails
    {
        long SourceId { get; }
        string SourceIdStr { get; }
        string SourceScreenName { get; }

        long TargetId { get; }
        string TargetIdStr { get; }
        string TargetScreenName { get; }

        bool Following { get; }
        bool FollowedBy { get; }
        bool FollowingRequestReceived { get; }
        bool FollowingRequested { get; }

        bool NotificationsEnabled { get; }
        bool CanSendDirectMessage { get; }

        bool Blocking { get; }
        bool BlockedBy { get; }
        bool Muting { get; }

        bool WantRetweets { get; }
        bool AllReplies { get; }
        bool MarkedSpam { get; }
    }
}