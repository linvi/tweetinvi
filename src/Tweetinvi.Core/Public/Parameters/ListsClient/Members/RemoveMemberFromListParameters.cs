using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy
    /// </summary>
    /// <inheritdoc />
    public interface IRemoveMemberFromListParameters : IListParameters
    {
        /// <summary>
        /// User to remove from list
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class RemoveMemberFromListParameters : TwitterListParameters, IRemoveMemberFromListParameters
    {
        public RemoveMemberFromListParameters(long listId, long userId) : this(new TwitterListIdentifier(listId), new UserIdentifier(userId))
        {
        }

        public RemoveMemberFromListParameters(long listId, string username) : this(new TwitterListIdentifier(listId), new UserIdentifier(username))
        {
        }

        public RemoveMemberFromListParameters(long listId, IUserIdentifier user) : this(new TwitterListIdentifier(listId), user)
        {
        }

        public RemoveMemberFromListParameters(ITwitterListIdentifier list, long userId) : this(list, new UserIdentifier(userId))
        {
        }

        public RemoveMemberFromListParameters(ITwitterListIdentifier list, string username) : this(list, new UserIdentifier(username))
        {
        }

        public RemoveMemberFromListParameters(ITwitterListIdentifier list, IUserIdentifier user) : base(list)
        {
            User = user;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}