using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserListsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The ID of the user for whom to return results.
        /// <para>If not specified, it will return the results for the account's user.</para>
        /// </summary>
        IUserIdentifier User { get; set; }

        /// <summary>
        /// Set this to true if you would like owned lists to be returned first.
        /// </summary>
        bool? Reverse { get; set; }
    }

    /// <inheritdoc />
    public class GetUserListsParameters : CustomRequestParameters, IGetUserListsParameters
    {
        public GetUserListsParameters()
        {
        }

        public GetUserListsParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserListsParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetUserListsParameters(IUserIdentifier user)
        {
            User = user;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
        /// <inheritdoc />
        public bool? Reverse { get; set; }
    }
}