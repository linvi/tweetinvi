using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface IUsersV2Client
    {
        Task<UserV2Response> GetUserAsync(long userId);
        Task<UserV2Response> GetUserAsync(string username);
        Task<UserV2Response> GetUserAsync(IGetUserByIdV2Parameters parameters);
        Task<UserV2Response> GetUserAsync(IGetUserByUsernameV2Parameters parameters);

        Task<UsersV2Response> GetUsersAsync(long[] userIds);
        Task<UsersV2Response> GetUsersAsync(string[] usernames);
        Task<UsersV2Response> GetUsersAsync(IGetUsersByIdV2Parameters parameters);
        Task<UsersV2Response> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters);
    }
}