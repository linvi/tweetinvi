using System.Threading.Tasks;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
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

        public AccountSettingsRequester(IAccountSettingsController accountSettingsController)
        {
            _accountSettingsController = accountSettingsController;
        }
        
        public Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return _accountSettingsController.UpdateProfileImage(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileBanner(IUpdateProfileBannerParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return _accountSettingsController.UpdateProfileBanner(parameters, request);
        }
    }
}