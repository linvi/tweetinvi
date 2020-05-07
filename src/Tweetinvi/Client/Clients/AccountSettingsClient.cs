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

        public Task<IAccountSettings> GetAccountSettingsAsync()
        {
            return GetAccountSettingsAsync(new GetAccountSettingsParameters());
        }

        public async Task<IAccountSettings> GetAccountSettingsAsync(IGetAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.GetAccountSettingsAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAccountSettings(twitterResult?.DataTransferObject);
        }

        public async Task<IAccountSettings> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateAccountSettingsAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAccountSettings(twitterResult?.DataTransferObject);
        }

        public async Task<IAuthenticatedUser> UpdateProfileAsync(IUpdateProfileParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfileAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAuthenticatedUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> UpdateProfileImageAsync(byte[] binary)
        {
            return UpdateProfileImageAsync(new UpdateProfileImageParameters(binary));
        }

        public async Task<IUser> UpdateProfileImageAsync(IUpdateProfileImageParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfileImageAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task UpdateProfileBannerAsync(byte[] binary)
        {
            return UpdateProfileBannerAsync(new UpdateProfileBannerParameters(binary));
        }

        public Task UpdateProfileBannerAsync(IUpdateProfileBannerParameters parameters)
        {
            return _accountRequester.UpdateProfileBannerAsync(parameters);
        }

        public Task RemoveProfileBannerAsync()
        {
            return RemoveProfileBannerAsync(new RemoveProfileBannerParameters());
        }

        public Task RemoveProfileBannerAsync(IRemoveProfileBannerParameters parameters)
        {
            return _accountRequester.RemoveProfileBannerAsync(parameters);
        }
    }
}