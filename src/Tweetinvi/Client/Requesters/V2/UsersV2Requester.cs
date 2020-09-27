using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public class UsersV2Requester : BaseRequester, IUsersV2Requester
    {
        private readonly IUsersV2Controller _usersV2Controller;

        public UsersV2Requester(
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents,
            IUsersV2Controller usersV2Controller) : base(client, twitterClientEvents)
        {
            _usersV2Controller = usersV2Controller;
        }

        public Task<ITwitterResult<UserResponseDTO>> GetUser(IGetUserV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _usersV2Controller.GetUserAsync(parameters, request));
        }

        public Task<ITwitterResult<UsersResponseDTO>> GetUsers(IGetUsersV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _usersV2Controller.GetUsersAsync(parameters, request));
        }
    }
}