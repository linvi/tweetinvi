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
            _accountRequester = client.RequestExecutor.AccountSettings;
        }

        public IAccountSettingsClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<IAccountSettings> GetAccountSettings()
        {
            return GetAccountSettings(new GetAccountSettingsParameters());
        }

        public async Task<IAccountSettings> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.GetAccountSettings(parameters).ConfigureAwait(false);
            return twitterResult.Result;
        }

        public async Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateAccountSettings(parameters).ConfigureAwait(false);
            return twitterResult.Result;
        }

        public async Task<IAuthenticatedUser> UpdateProfile(IUpdateProfileParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfile(parameters).ConfigureAwait(false);
            return twitterResult.Result;
        }

        public Task UpdateProfileImage(byte[] binary)
        {
            return UpdateProfileImage(new UpdateProfileImageParameters(binary));
        }

        public async Task UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            await _accountRequester.UpdateProfileImage(parameters).ConfigureAwait(false);
        }

        public Task UpdateProfileBanner(byte[] binary)
        {
            return UpdateProfileBanner(new UpdateProfileBannerParameters(binary));
        }

        public async Task UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            await _accountRequester.UpdateProfileBanner(parameters).ConfigureAwait(false);
        }

        public Task RemoveProfileBanner()
        {
            return RemoveProfileBanner(new RemoveProfileBannerParameters());
        }

        public async Task RemoveProfileBanner(IRemoveProfileBannerParameters parameters)
        {
            await _accountRequester.RemoveProfileBanner(parameters).ConfigureAwait(false);
        }
    }
}