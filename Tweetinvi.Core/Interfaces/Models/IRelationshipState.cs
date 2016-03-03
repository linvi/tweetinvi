namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IRelationshipState
    {
        /// <summary>
        /// User id of the relationship target.
        /// </summary>
        long TargetId { get; }

        /// <summary>
        /// User id of the relationship target.
        /// </summary>
        string TargetIdStr { get; }

        /// <summary>
        /// User display name of the relationship target.
        /// </summary>
        string TargetName { get; }

        /// <summary>
        /// User screen name of the relationship target.
        /// </summary>
        string TargetScreenName { get; }

        /// <summary>
        /// Is the source target following the target.
        /// </summary>
        bool Following { get; }

        /// <summary>
        /// Is the source target followed by the target.
        /// </summary>
        bool FollowedBy { get; }

        
        /// <summary>
        /// Has a following request been sent to the target.
        /// </summary>
        bool FollowingRequested { get; }

        /// <summary>
        /// Has a following request been received by the source.
        /// </summary>
        bool FollowingRequestReceived { get; }
    }
}