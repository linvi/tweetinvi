using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
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

        public Task<bool> UpdateProfileImage(byte[] binary)
        {
            return UpdateProfileImage(new UpdateProfileImageParameters(binary));
        }

        public async Task<bool> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            return (await _accountRequester.UpdateProfileImage(parameters).ConfigureAwait(false)).Response.IsSuccessStatusCode;
        }
    }
}