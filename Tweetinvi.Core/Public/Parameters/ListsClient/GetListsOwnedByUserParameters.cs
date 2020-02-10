using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsOwnedByUserParameters : IGetListsOwnedByAccountParameters
    {
        /// <summary>
        /// User from whom we want to get the owned lists
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetListsOwnedByAccountByUserParameters : GetListsOwnedByAccountParameters, IGetListsOwnedByUserParameters
    {
        /// <inheritdoc />
        public IUserIdentifier User { get; set; }

        public GetListsOwnedByAccountByUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetListsOwnedByAccountByUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetListsOwnedByAccountByUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetListsOwnedByAccountByUserParameters(IGetListsOwnedByAccountParameters parameters) : base(parameters)
        {
        }

        public GetListsOwnedByAccountByUserParameters(IGetListsOwnedByUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }
    }
}