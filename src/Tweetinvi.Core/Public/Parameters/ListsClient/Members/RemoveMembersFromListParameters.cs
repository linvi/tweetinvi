using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy_all
    /// </summary>
    /// <inheritdoc />
    public interface IRemoveMembersFromListParameters : IListParameters
    {
        /// <summary>
        /// Users to remove from the list
        /// </summary>
        List<IUserIdentifier> Users { get; set; }
    }

    /// <inheritdoc />
    public class RemoveMembersFromListParameters : TwitterListParameters, IRemoveMembersFromListParameters
    {
        public RemoveMembersFromListParameters(long listId, IEnumerable<long> userIds)
            : this(new TwitterListIdentifier(listId), userIds.Select(x => new UserIdentifier(x)))
        {
        }

        public RemoveMembersFromListParameters(long listId, IEnumerable<string> usernames)
            : this(new TwitterListIdentifier(listId), usernames.Select(x => new UserIdentifier(x)))
        {
        }

        public RemoveMembersFromListParameters(long listId, IEnumerable<IUserIdentifier> users) : this(new TwitterListIdentifier(listId), users)
        {
        }

        public RemoveMembersFromListParameters(ITwitterListIdentifier list, IEnumerable<long> userIds)
            : this(list, userIds.Select(x => new UserIdentifier(x)))
        {
        }

        public RemoveMembersFromListParameters(ITwitterListIdentifier list, IEnumerable<string> usernames)
            : this(list, usernames.Select(x => new UserIdentifier(x)))
        {
        }

        public RemoveMembersFromListParameters(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users) : base(list)
        {
            Users.AddRange(users);
        }

        /// <inheritdoc />
        public List<IUserIdentifier> Users { get; set; } = new List<IUserIdentifier>();
    }
}