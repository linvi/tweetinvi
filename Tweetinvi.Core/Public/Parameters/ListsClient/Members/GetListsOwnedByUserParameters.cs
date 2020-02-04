using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsOwnedByUserParameters : IGetListsOwnedParameters
    {
        /// <summary>
        /// User from whom we want to get the owned lists
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetListsOwnedByUserParameters : GetListsOwnedParameters, IGetListsOwnedByUserParameters
    {
        /// <inheritdoc />
        public IUserIdentifier User { get; set; }

        public GetListsOwnedByUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetListsOwnedByUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetListsOwnedByUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetListsOwnedByUserParameters(IGetListsOwnedByUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }
    }
}