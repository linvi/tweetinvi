using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface IUsersV2Client
    {
        /// <inheritdoc cref="GetUserByIdAsync(IGetUserByIdV2Parameters)"/>
        Task<UserV2Response> GetUserByIdAsync(long userId);
        /// <inheritdoc cref="GetUserByIdAsync(IGetUserByIdV2Parameters)"/>
        Task<UserV2Response> GetUserByIdAsync(string userId);

        /// <summary>
        /// Get a user from his id
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-id </para>
        /// </summary>
        /// <returns>Returns the requested user</returns>
        Task<UserV2Response> GetUserByIdAsync(IGetUserByIdV2Parameters parameters);

        /// <inheritdoc cref="GetUserByNameAsync(IGetUserByNameV2Parameters)"/>
        Task<UserV2Response> GetUserByNameAsync(string username);

        /// <summary>
        /// Get a user from his screen name
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by-username-username </para>
        /// </summary>
        /// <returns>Returns the requested user</returns>
        Task<UserV2Response> GetUserByNameAsync(IGetUserByNameV2Parameters parameters);

        /// <inheritdoc cref="GetUsersByIdAsync(IGetUsersByIdV2Parameters)"/>
        Task<UsersV2Response> GetUsersByIdAsync(params long[] userIds);
        /// <inheritdoc cref="GetUsersByIdAsync(IGetUsersByIdV2Parameters)"/>
        Task<UsersV2Response> GetUsersByIdAsync(params string[] userIds);

        /// <summary>
        /// Get users from their ids
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users </para>
        /// </summary>
        /// <returns>Returns the requested users</returns>
        Task<UsersV2Response> GetUsersByIdAsync(IGetUsersByIdV2Parameters parameters);

        /// <inheritdoc cref="GetUsersByNameAsync(IGetUsersByNameV2Parameters)"/>
        Task<UsersV2Response> GetUsersByNameAsync(params string[] usernames);

        /// <summary>
        /// Get users from their screen names
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by </para>
        /// </summary>
        /// <returns>Returns the requested users</returns>
        Task<UsersV2Response> GetUsersByNameAsync(IGetUsersByNameV2Parameters parameters);
    }
}