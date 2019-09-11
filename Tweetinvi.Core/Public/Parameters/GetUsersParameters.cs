using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetUsersParameters : ICustomRequestParameters
    {
        IUserIdentifier[] UserIdentifiers { get; set; }
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    public class GetUsersParameters : CustomRequestParameters, IGetUsersParameters
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
            if (source == null) { return; }

            UserIdentifiers = source.UserIdentifiers;
            IncludeEntities = source.IncludeEntities;
        }

        public IUserIdentifier[] UserIdentifiers { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}
