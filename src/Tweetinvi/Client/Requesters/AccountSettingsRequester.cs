using Tweetinvi.Controllers.AccountSettings;
using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class AccountSettingsRequester : BaseRequester, IAccountSettingsRequester
    {
        private readonly IAccountSettingsController _accountSettingsController;
        private readonly IAccountSettingsClientRequiredParametersValidator _validator;

        public AccountSettingsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IAccountSettingsController accountSettingsController,
            IAccountSettingsClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _accountSettingsController = accountSettingsController;
            _validator = validator;
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.GetAccountSettings(parameters, request));
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.UpdateAccountSettings(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfile(IUpdateProfileParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.UpdateProfile(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.UpdateProfileImage(parameters, request));
        }

        public Task<ITwitterResult> UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.UpdateProfileBanner(parameters, request));
        }

        public Task<ITwitterResult> RemoveProfileBanner(IRemoveProfileBannerParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountSettingsController.RemoveProfileBanner(parameters, request));
        }
    }
}