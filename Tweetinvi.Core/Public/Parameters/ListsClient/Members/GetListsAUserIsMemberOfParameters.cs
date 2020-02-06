using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsAUserIsMemberOfParameters : IGetListsAccountIsMemberOfParameters
    {
        /// <summary>
        /// User from whom we want to get the lists he is a member of
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetListsAUserIsMemberOfParameters : GetListsAccountIsMemberOfParameters, IGetListsAUserIsMemberOfParameters
    {
        public GetListsAUserIsMemberOfParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetListsAUserIsMemberOfParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetListsAUserIsMemberOfParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetListsAUserIsMemberOfParameters(IGetListsAccountIsMemberOfParameters parameters) : base(parameters)
        {
        }

        public GetListsAUserIsMemberOfParameters(IGetListsAUserIsMemberOfParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}