using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsSubscribedByUserParameters : IGetListsSubscribedByAccountParameters
    {
        /// <summary>
        /// The ID of the user for whom to return results.
        /// <para>If not specified, it will return the results for the account's user.</para>
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc />
    public class GetListsSubscribedByUserParameters : GetListsSubscribedByAccountParameters, IGetListsSubscribedByUserParameters
    {
        public GetListsSubscribedByUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetListsSubscribedByUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetListsSubscribedByUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetListsSubscribedByUserParameters(IGetListsSubscribedByUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        public GetListsSubscribedByUserParameters(IGetListsSubscribedByAccountParameters parameters) : base(parameters)
        {
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}