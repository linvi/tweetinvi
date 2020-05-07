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

        public Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettingsAsync(IGetAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.GetAccountSettingsAsync(parameters, request));
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.UpdateAccountSettingsAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileAsync(IUpdateProfileParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.UpdateProfileAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileImageAsync(IUpdateProfileImageParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.UpdateProfileImageAsync(parameters, request));
        }

        public Task<ITwitterResult> UpdateProfileBannerAsync(IUpdateProfileBannerParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.UpdateProfileBannerAsync(parameters, request));
        }

        public Task<ITwitterResult> RemoveProfileBannerAsync(IRemoveProfileBannerParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountSettingsController.RemoveProfileBannerAsync(parameters, request));
        }
    }
}