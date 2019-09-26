using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
    /// </summary>
    /// <inheritdoc />
    public interface IGetFollowerIdsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// A unique identifier of a user
        /// </summary>
        IUserIdentifier UserIdentifier { get; }
    }

    /// <inheritdoc />
    public class GetFollowerIdsParameters : CursorQueryParameters, IGetFollowerIdsParameters
    {
        private GetFollowerIdsParameters()
        {
            PageSize = 5000;
        }

        public GetFollowerIdsParameters(IUserIdentifier userIdentifier) : this()
        {
            UserIdentifier = userIdentifier;
        }

        public GetFollowerIdsParameters(string username) : this()
        {
            UserIdentifier = new UserIdentifier(username);
        }

        public GetFollowerIdsParameters(long? userId) : this()
        {
            UserIdentifier = new UserIdentifier(userId);
        }
        
        public GetFollowerIdsParameters(IGetFollowerIdsParameters parameters) : base(parameters)
        {
            if (parameters == null) { return; }
            
            UserIdentifier = parameters.UserIdentifier;
        }

        /// <inheritdoc />
        public IUserIdentifier UserIdentifier { get; }
    }
}
