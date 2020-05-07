using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsController
    {
        Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettingsAsync(IGetAccountSettingsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IAccountSettingsDTO>> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UpdateProfileAsync(IUpdateProfileParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UpdateProfileImageAsync(IUpdateProfileImageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> UpdateProfileBannerAsync(IUpdateProfileBannerParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RemoveProfileBannerAsync(IRemoveProfileBannerParameters parameters, ITwitterRequest request);
    }

    public class AccountSettingsController : IAccountSettingsController
    {
        private readonly IAccountSettingsQueryExecutor _accountSettingsQueryExecutor;

        public AccountSettingsController(IAccountSettingsQueryExecutor accountSettingsQueryExecutor)
        {
            _accountSettingsQueryExecutor = accountSettingsQueryExecutor;
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettingsAsync(IGetAccountSettingsParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.GetAccountSettingsAsync(parameters, request);
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateAccountSettingsAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileAsync(IUpdateProfileParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateProfileAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileImageAsync(IUpdateProfileImageParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateProfileImageAsync(parameters, request);
        }

        public Task<ITwitterResult> UpdateProfileBannerAsync(IUpdateProfileBannerParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.UpdateProfileBannerAsync(parameters, request);
        }

        public Task<ITwitterResult> RemoveProfileBannerAsync(IRemoveProfileBannerParameters parameters, ITwitterRequest request)
        {
            return _accountSettingsQueryExecutor.RemoveProfileBannerAsync(parameters, request);
        }
    }
}