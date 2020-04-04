using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountSettingsClient : IAccountSettingsClient
    {
        private readonly ITwitterClient _client;
        private readonly IAccountSettingsRequester _accountRequester;

        public AccountSettingsClient(ITwitterClient client)
        {
            _client = client;
            _accountRequester = client.Raw.AccountSettings;
        }

        public IAccountSettingsClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<IAccountSettings> GetAccountSettings()
        {
            return GetAccountSettings(new GetAccountSettingsParameters());
        }

        public async Task<IAccountSettings> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.GetAccountSettings(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAccountSettings(twitterResult?.DataTransferObject);
        }

        public async Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateAccountSettings(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAccountSettings(twitterResult?.DataTransferObject);
        }

        public async Task<IAuthenticatedUser> UpdateProfile(IUpdateProfileParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfile(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAuthenticatedUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> UpdateProfileImage(byte[] binary)
        {
            return UpdateProfileImage(new UpdateProfileImageParameters(binary));
        }

        public async Task<IUser> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfileImage(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task UpdateProfileBanner(byte[] binary)
        {
            return UpdateProfileBanner(new UpdateProfileBannerParameters(binary));
        }

        public Task UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            return _accountRequester.UpdateProfileBanner(parameters);
        }

        public Task RemoveProfileBanner()
        {
            return RemoveProfileBanner(new RemoveProfileBannerParameters());
        }

        public Task RemoveProfileBanner(IRemoveProfileBannerParameters parameters)
        {
            return _accountRequester.RemoveProfileBanner(parameters);
        }
    }
}