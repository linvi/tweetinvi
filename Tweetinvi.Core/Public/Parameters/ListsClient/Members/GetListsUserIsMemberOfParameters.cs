using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsUserIsMemberOfParameters : IGetListsAccountIsMemberOfParameters
    {
        /// <summary>
        /// User from whom we want to get the lists he is a member of
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetListsUserIsMemberOfParameters : GetListsAccountIsMemberOfParameters, IGetListsUserIsMemberOfParameters
    {
        public GetListsUserIsMemberOfParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetListsUserIsMemberOfParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetListsUserIsMemberOfParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetListsUserIsMemberOfParameters(IGetListsUserIsMemberOfParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}