using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsController
    {
        Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UpdateProfileBanner(IUpdateProfileBannerParameters parameters, ITwitterRequest request);
    }
    
    public class AccountSettingsController : IAccountSettingsController
    {
        private readonly IAccountSettingsQueryExecutor _accountSettingsQueryExecutor;

        public AccountSettingsController(IAccountSettingsQueryExecutor accountSettingsQueryExecutor)
        {
            _accountSettingsQueryExecutor = accountSettingsQueryExecutor;
        }
        
        public Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateProfileImage(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileBanner(IUpdateProfileBannerParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateProfileBanner(parameters, request);
        }
    }
}