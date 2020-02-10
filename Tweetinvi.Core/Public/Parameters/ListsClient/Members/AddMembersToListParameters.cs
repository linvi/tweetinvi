using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create_all
    /// </summary>
    /// <inheritdoc />
    public interface IAddMembersToListParameters : IListParameters
    {
        /// <summary>
        /// List of users to be added to the list
        /// </summary>
        List<IUserIdentifier> Users { get; set; }
    }

    /// <inheritdoc />
    public class AddMembersToListParameters : ListParameters, IAddMembersToListParameters
    {
        public AddMembersToListParameters(long? listId, IEnumerable<long?> userIds)
            : this(new TwitterListIdentifier(listId), userIds.Select(x => new UserIdentifier(x)))
        {
        }

        public AddMembersToListParameters(long? listId, IEnumerable<string> usernames)
            : this(new TwitterListIdentifier(listId), usernames.Select(x => new UserIdentifier(x)))
        {
        }

        public AddMembersToListParameters(long? listId, IEnumerable<IUserIdentifier> users) : this(new TwitterListIdentifier(listId), users)
        {
        }

        public AddMembersToListParameters(ITwitterListIdentifier list, IEnumerable<long?> userIds)
            : this(list, userIds.Select(x => new UserIdentifier(x)))
        {
        }

        public AddMembersToListParameters(ITwitterListIdentifier list, IEnumerable<string> usernames)
            : this(list, usernames.Select(x => new UserIdentifier(x)))
        {
        }

        public AddMembersToListParameters(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users) : base(list)
        {
            Users.AddRange(users);
        }

        /// <inheritdoc />
        public List<IUserIdentifier> Users { get; set; } = new List<IUserIdentifier>();
    }
}