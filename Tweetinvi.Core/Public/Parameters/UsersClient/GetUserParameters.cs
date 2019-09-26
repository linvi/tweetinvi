using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-show
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserParameters : IGetUsersOptionalParameters
    {
        /// <summary>
        /// User identifier
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }
    }

    /// <inheritdoc />
    public class GetUserParameters : GetUsersOptionalParameters, IGetUserParameters
    {
        public GetUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserParameters(string username) : this(new UserIdentifier(username))
        {
        }
        
        public GetUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public GetUserParameters(IGetUserParameters source) : base(source)
        {
            UserIdentifier = source?.UserIdentifier;
        }

        /// <inheritdoc />
        public IUserIdentifier UserIdentifier { get; set; }
    }
}