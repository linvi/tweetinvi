using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers-show
    /// </summary>
    public interface ICheckIfUserIsSubscriberOfListParameters : IListParameters
    {
        /// <summary>
        /// User for whom we want to verify the subscription
        /// </summary>
        IUserIdentifier User { get; set; }

        /// <summary>
        /// Each tweet will include a node called "entities". This property offers a variety of
        /// metadata about the tweet in a discreet structure, including: user_mentions, urls, and hashtags.
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// Statuses will not be included in the returned user objects.
        /// </summary>
        bool? SkipStatus { get; set; }
    }

    /// <inheritdoc />
    public class CheckIfUserIsSubscriberOfListParameters : ListParameters, ICheckIfUserIsSubscriberOfListParameters
    {
        public CheckIfUserIsSubscriberOfListParameters(long? listId, long userId) : this(new TwitterListIdentifier(listId), new UserIdentifier(userId))
        {
        }

        public CheckIfUserIsSubscriberOfListParameters(long? listId, string username) : this(new TwitterListIdentifier(listId), new UserIdentifier(username))
        {
        }

        public CheckIfUserIsSubscriberOfListParameters(long? listId, IUserIdentifier user) : this(new TwitterListIdentifier(listId), user)
        {
        }

        public CheckIfUserIsSubscriberOfListParameters(ITwitterListIdentifier list, long userId) : this(list, new UserIdentifier(userId))
        {
        }

        public CheckIfUserIsSubscriberOfListParameters(ITwitterListIdentifier list, string username) : this(list, new UserIdentifier(username))
        {
        }

        public CheckIfUserIsSubscriberOfListParameters(ITwitterListIdentifier list, IUserIdentifier user) : base(list)
        {
            User = user;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
        /// <inheritdoc />
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc />
        public bool? SkipStatus { get; set; }
    }
}