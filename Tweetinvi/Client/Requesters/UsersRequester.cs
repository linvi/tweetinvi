using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalUsersRequester : IUsersRequester, IBaseRequester
    {
    }

    public interface IUsersRequester
    {
        Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters);
    }

    public class UsersRequester : BaseRequester, IInternalUsersRequester
    {
        private readonly IUserController _userController;

        public UsersRequester(IUserController userController)
        {
            _userController = userController;
        }

        public Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _userController.GetAuthenticatedUser(parameters, request), request);
        }
    }
}
