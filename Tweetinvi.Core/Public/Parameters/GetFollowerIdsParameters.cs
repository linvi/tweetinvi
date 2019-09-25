using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// Parameters to get a user's list of friends
    /// </summary>
    public interface IGetFollowerIdsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// A unique identifier of a user
        /// </summary>
        IUserIdentifier UserIdentifier { get; }
    }

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

        public GetFollowerIdsParameters(long userId) : this()
        {
            UserIdentifier = new UserIdentifier(userId);
        }
        
        public GetFollowerIdsParameters(IGetFollowerIdsParameters parameters) : base(parameters)
        {
            if (parameters == null) { return; }
            
            UserIdentifier = parameters.UserIdentifier;
        }

        public IUserIdentifier UserIdentifier { get; }
    }
}
