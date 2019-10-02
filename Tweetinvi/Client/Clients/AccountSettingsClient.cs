using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountSettingsClient : IAccountSettingsClient
    {
        private readonly IAccountSettingsRequester _accountRequester;

        public AccountSettingsClient(TwitterClient client)
        {
            _accountRequester = client.RequestExecutor.AccountSettings;
        }

        public Task<IAccountSettings> GetAccountSettings()
        {
            return GetAccountSettings(new GetAccountSettingsParameters());
        }
        
        public async Task<IAccountSettings> GetAccountSettings(IGetAccountSettingsParameters parameters)
        {
            var twitterResult = await _accountRequester.GetAccountSettings(parameters).ConfigureAwait(false);
            return twitterResult.Result;
        }
        
        public Task<bool> UpdateProfileImage(byte[] binary)
        {
            return UpdateProfileImage(new UpdateProfileImageParameters(binary));
        }

        public async Task<bool> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            return (await _accountRequester.UpdateProfileImage(parameters).ConfigureAwait(false)).Response.IsSuccessStatusCode;
        }
        
        public Task<bool> UpdateProfileBanner(byte[] binary)
        {
            return UpdateProfileBanner(new UpdateProfileBannerParameters(binary));
        }

        public async Task<bool> UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            var twitterResult = await _accountRequester.UpdateProfileBanner(parameters).ConfigureAwait(false);
            return twitterResult.Response.IsSuccessStatusCode;
        }

        public Task<bool> RemoveProfileBanner()
        {
            return RemoveProfileBanner(new RemoveProfileBannerParameters());
        }

        public async Task<bool> RemoveProfileBanner(IRemoveProfileBannerParameters parameters)
        {
            var twitterResult = await _accountRequester.RemoveProfileBanner(parameters).ConfigureAwait(false);
            return twitterResult.Response.IsSuccessStatusCode;
        }
    }
}