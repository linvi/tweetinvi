using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserListMembershipsParameters : IGetAccountListMembershipsParameters
    {
        /// <summary>
        /// User from whom we want to get the lists he is a member of
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetUserListMembershipsParameters : GetAccountListMembershipsParameters, IGetUserListMembershipsParameters
    {
        public GetUserListMembershipsParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserListMembershipsParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetUserListMembershipsParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetUserListMembershipsParameters(IGetAccountListMembershipsParameters parameters) : base(parameters)
        {
        }

        public GetUserListMembershipsParameters(IGetUserListMembershipsParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}