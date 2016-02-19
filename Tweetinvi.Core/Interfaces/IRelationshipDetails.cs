namespace Tweetinvi.Core.Interfaces
{
    public interface IRelationshipDetails
    {
        /// <summary>
        /// The source user of the relationship
        /// </summary>
        long SourceId { get; }

        /// <summary>
        /// The source user of the relationship
        /// </summary>
        string SourceIdStr { get; }

        /// <summary>
        /// The source user of the relationship
        /// </summary>
        string SourceScreenName { get; }

        /// <summary>
        /// The target user of the relationship
        /// </summary>
        long TargetId { get; }
        
        /// <summary>
        /// The target user of the relationship
        /// </summary>
        string TargetIdStr { get; }

        /// <summary>
        /// The target user of the relationship
        /// </summary>
        string TargetScreenName { get; }

        /// <summary>
        /// Informs if the source user following the target user.
        /// </summary>
        bool Following { get; }

        /// <summary>
        /// Informs if the source user is followed by the target user.
        /// </summary>
        bool FollowedBy { get; }

        /// <summary>
        /// Informs if the private source user has received a request to be followed by the target user.
        /// </summary>
        bool FollowingRequestReceived { get; }

        /// <summary>
        /// Informs if the source user requested to follow the private target user.
        /// </summary>
        bool FollowingRequested { get; }

        /// <summary>
        /// Informs if the source user has requested to be notified when the target user publishes tweets. 
        /// </summary>
        bool NotificationsEnabled { get; }

        /// <summary>
        /// Informs if the source user can send private messages to the target.
        /// </summary>
        bool CanSendDirectMessage { get; }

        /// <summary>
        /// Informs if the source has blocked the target.
        /// </summary>
        bool Blocking { get; }

        bool BlockedBy { get; }

        /// <summary>
        /// Informs if the source has muted the target.
        /// </summary>
        bool Muting { get; }

        /// <summary>
        /// Informs if the source wants to receive a notification for retweets published by the target.
        /// </summary>
        bool WantRetweets { get; }

        /// <summary>
        /// Informs if the source wants to receive a notification for each reply published by the target.
        /// </summary>
        bool AllReplies { get; }

        /// <summary>
        /// Informs if the source has marked the user as being a spammer.
        /// </summary>
        bool MarkedSpam { get; }
    }
}