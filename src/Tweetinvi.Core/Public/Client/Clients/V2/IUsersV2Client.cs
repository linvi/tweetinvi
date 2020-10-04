using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface IUsersV2Client
    {
        /// <inheritdoc cref="GetUserAsync(IGetUserByIdV2Parameters)"/>
        Task<UserV2Response> GetUserAsync(long userId);

        /// <summary>
        /// Get a user from his id
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-id </para>
        /// </summary>
        /// <returns>Returns the requested user</returns>
        Task<UserV2Response> GetUserAsync(IGetUserByIdV2Parameters parameters);

        /// <inheritdoc cref="GetUserAsync(IGetUserByUsernameV2Parameters)"/>
        Task<UserV2Response> GetUserAsync(string username);

        /// <summary>
        /// Get a user from his screen name
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by-username-username </para>
        /// </summary>
        /// <returns>Returns the requested user</returns>
        Task<UserV2Response> GetUserAsync(IGetUserByUsernameV2Parameters parameters);

        /// <inheritdoc cref="GetUsersAsync(IGetUsersByIdV2Parameters)"/>
        Task<UsersV2Response> GetUsersAsync(long[] userIds);

        /// <summary>
        /// Get users from their ids
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users </para>
        /// </summary>
        /// <returns>Returns the requested users</returns>
        Task<UsersV2Response> GetUsersAsync(IGetUsersByIdV2Parameters parameters);

        /// <inheritdoc cref="GetUsersAsync(IGetUsersByUsernameV2Parameters)"/>
        Task<UsersV2Response> GetUsersAsync(string[] usernames);

        /// <summary>
        /// Get users from their screen names
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/users/lookup/api-reference/get-users-by </para>
        /// </summary>
        /// <returns>Returns the requested users</returns>
        Task<UsersV2Response> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters);
    }
}