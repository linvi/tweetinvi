using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-lookup
    /// </summary>
    /// <inheritdoc />
    public interface IGetRelationshipsWithParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Collection of users for whom you want to check the relationship of the client's user
        /// </summary>
        IUserIdentifier[] Users { get; set; }
    }

    /// <inheritdoc />
    public class GetRelationshipsWithParameters : CustomRequestParameters, IGetRelationshipsWithParameters
    {
        public GetRelationshipsWithParameters(long?[] userIds)
        {
            Users = userIds.Select(x => new UserIdentifier(x)).Cast<IUserIdentifier>().ToArray();
        }

        public GetRelationshipsWithParameters(long[] userIds)
        {
            Users = userIds.Select(x => new UserIdentifier(x)).Cast<IUserIdentifier>().ToArray();
        }

        public GetRelationshipsWithParameters(string[] usernames)
        {
            Users = usernames.Select(x => new UserIdentifier(x)).Cast<IUserIdentifier>().ToArray();
        }

        public GetRelationshipsWithParameters(IUserIdentifier[] users)
        {
            Users = users;
        }

        public GetRelationshipsWithParameters(IUser[] users)
        {
            Users = users.Cast<IUserIdentifier>().ToArray();
        }

        public GetRelationshipsWithParameters(IGetRelationshipsWithParameters source) : base(source)
        {
            Users = source?.Users;
        }

        /// <inheritdoc />
        public IUserIdentifier[] Users { get; set; }
    }
}
