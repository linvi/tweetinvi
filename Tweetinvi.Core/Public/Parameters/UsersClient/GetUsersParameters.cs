using System.Linq;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup
    /// </summary>
    /// <inheritdoc />
    public interface IGetUsersParameters : IGetUsersOptionalParameters
    {
        /// <summary>
        /// User identifiers
        /// </summary>
        IUserIdentifier[] UserIdentifiers { get; set; }
    }

    /// <inheritdoc />
    public class GetUsersParameters : GetUsersOptionalParameters, IGetUsersParameters
    {
        public GetUsersParameters(long[] userIds)
        {
            UserIdentifiers = userIds.Select(userId => new UserIdentifier(userId) as IUserIdentifier).ToArray();
        }
        
        public GetUsersParameters(long?[] userIds)
        {
            UserIdentifiers = userIds.Select(userId => new UserIdentifier(userId) as IUserIdentifier).ToArray();
        }

        public GetUsersParameters(string[] usernames)
        {
            UserIdentifiers = usernames.Select(username => new UserIdentifier(username) as IUserIdentifier).ToArray();
        }

        public GetUsersParameters(IUserIdentifier[] userIdentifiers)
        {
            UserIdentifiers = userIdentifiers;
        }

        public GetUsersParameters(IGetUsersParameters source) : base(source)
        {
            UserIdentifiers = source?.UserIdentifiers;
        }

        /// <inheritdoc />
        public IUserIdentifier[] UserIdentifiers { get; set; }
    }
}
