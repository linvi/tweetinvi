namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IRelationshipState
    {
        long TargetId { get; }
        string TargetIdStr { get; }

        string TargetName { get; }
        string TargetScreenName { get; }

        bool Following { get; }
        bool FollowedBy { get; }
        bool FollowingRequested { get; }
        bool FollowingRequestReceived { get; }
    }
}