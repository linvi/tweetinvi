using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class AccountSettingsRequester : BaseRequester, IAccountSettingsRequester
    {
        private readonly ITwitterClientFactories _factories;
        private readonly IAccountSettingsController _accountSettingsController;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IAccountSettingsClientRequiredParametersValidator _validator;

        public AccountSettingsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITwitterClientFactories factories,
            IAccountSettingsController accountSettingsController,
            ITwitterResultFactory twitterResultFactory,
            IAccountSettingsClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _factories = factories;
            _accountSettingsController = accountSettingsController;
            _twitterResultFactory = twitterResultFactory;
            _validator = validator;
        }

        public Task<ITwitterResult<IAccountSettingsDTO, IAccountSettings>> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _accountSettingsController.GetAccountSettings(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create<IAccountSettingsDTO, IAccountSettings>(twitterResult, dto => new AccountSettings(dto));
            });
        }

        public Task<ITwitterResult<IAccountSettingsDTO, IAccountSettings>> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _accountSettingsController.UpdateAccountSettings(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create<IAccountSettingsDTO, IAccountSettings>(twitterResult, dto => new AccountSettings(dto));
            });
        }

        public Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> UpdateProfile(IUpdateProfileParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _accountSettingsController.UpdateProfile(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto =>
                {
                    var user = _factories.CreateAuthenticatedUser(dto);
                    return user;
                });
            });
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