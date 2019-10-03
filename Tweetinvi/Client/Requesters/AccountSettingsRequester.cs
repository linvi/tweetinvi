using System.Threading.Tasks;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.Model;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalAccountSettingsRequester : IAccountSettingsRequester, IBaseRequester
    {
    }
    
    public class AccountSettingsRequester : BaseRequester, IInternalAccountSettingsRequester
    {
        private readonly IAccountSettingsController _accountSettingsController;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public AccountSettingsRequester(
            IAccountSettingsController accountSettingsController,
            ITwitterResultFactory twitterResultFactory)
        {
            _accountSettingsController = accountSettingsController;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IAccountSettingsDTO, IAccountSettings>> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var twitterResult = await _accountSettingsController.GetAccountSettings(parameters, request).ConfigureAwait(false);
            return _twitterResultFactory.Create<IAccountSettingsDTO, IAccountSettings>(twitterResult, dto => new AccountSettings(dto));
        }

        public async Task<ITwitterResult<IAccountSettingsDTO, IAccountSettings>> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var twitterResult = await _accountSettingsController.UpdateAccountSettings(parameters, request).ConfigureAwait(false);
            return _twitterResultFactory.Create<IAccountSettingsDTO, IAccountSettings>(twitterResult, dto => new AccountSettings(dto));
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return _accountSettingsController.UpdateProfileImage(parameters, request);
        }

        public Task<ITwitterResult> UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return _accountSettingsController.UpdateProfileBanner(parameters, request);
        }

        public Task<ITwitterResult> RemoveProfileBanner(IRemoveProfileBannerParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return _accountSettingsController.RemoveProfileBanner(parameters, request);
        }
    }
}