using System.Linq;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    public interface IGetUsersParameters : IGetUsersOptionalParameters
    {
        IUserIdentifier[] UserIdentifiers { get; set; }
    }

    public class GetUsersParameters : GetUsersOptionalParameters, IGetUsersParameters
    {
        public GetUsersParameters(long[] userIds)
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

        public IUserIdentifier[] UserIdentifiers { get; set; }
    }
}
