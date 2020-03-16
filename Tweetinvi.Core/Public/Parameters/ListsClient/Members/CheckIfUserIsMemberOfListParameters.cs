using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members-show
    /// </summary>
    /// <inheritdoc />
    public interface ICheckIfUserIsMemberOfListParameters : IListParameters
    {
        /// <summary>
        /// User for whom we want to verify the membership
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
    public class CheckIfUserIsMemberOfListParameters : ListParameters, ICheckIfUserIsMemberOfListParameters
    {
        public CheckIfUserIsMemberOfListParameters(long listId, long userId) : this(new TwitterListIdentifier(listId), new UserIdentifier(userId))
        {
        }

        public CheckIfUserIsMemberOfListParameters(long listId, string username) : this(new TwitterListIdentifier(listId), new UserIdentifier(username))
        {
        }

        public CheckIfUserIsMemberOfListParameters(long listId, IUserIdentifier user) : this(new TwitterListIdentifier(listId), user)
        {
        }

        public CheckIfUserIsMemberOfListParameters(ITwitterListIdentifier list, long userId) : this(list, new UserIdentifier(userId))
        {
        }

        public CheckIfUserIsMemberOfListParameters(ITwitterListIdentifier list, string username) : this(list, new UserIdentifier(username))
        {
        }

        public CheckIfUserIsMemberOfListParameters(ITwitterListIdentifier list, IUserIdentifier user) : base(list)
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