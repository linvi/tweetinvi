using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Upload;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsQueryExecutor
    {
        Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettings(IGetAccountSettingsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> UpdateProfileBanner(IUpdateProfileBannerParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RemoveProfileBanner(IRemoveProfileBannerParameters parameters, ITwitterRequest request);
    }
    
    public class AccountSettingsQueryExecutor : IAccountSettingsQueryExecutor
    {
        private readonly IAccountSettingsQueryGenerator _accountSettingsQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public AccountSettingsQueryExecutor(
            IAccountSettingsQueryGenerator accountSettingsQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _accountSettingsQueryGenerator = accountSettingsQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<IAccountSettingsDTO>> GetAccountSettings(IGetAccountSettingsParameters parameters, ITwitterRequest request)
        {
            var query = _accountSettingsQueryGenerator.GetAccountSettingsQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IAccountSettingsDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters, ITwitterRequest request)
        {
            var query = _accountSettingsQueryGenerator.GetUpdateProfileImageQuery(parameters);

            var multipartQuery = new MultipartTwitterQuery(request.Query)
            {
                Url = query,
                HttpMethod = HttpMethod.POST,
                Binaries = new[] { parameters.Binary },
                ContentId = "image",
                Timeout = parameters.Timeout ?? TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite),
                UploadProgressChanged = parameters.UploadProgressChanged,
            };

            request.Query = multipartQuery;
            
            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult> UpdateProfileBanner(IUpdateProfileBannerParameters parameters, ITwitterRequest request)
        {
            var query = _accountSettingsQueryGenerator.GetUpdateProfileBannerQuery(parameters);
            var banner = StringFormater.UrlEncode(Convert.ToBase64String(parameters.Binary));
            var bannerHttpContent = new StringContent($"banner={banner}", Encoding.UTF8, "application/x-www-form-urlencoded");

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.HttpContent = new ProgressableStreamContent(bannerHttpContent, parameters.UploadProgressChanged);
            request.Query.IsHttpContentPartOfQueryParams = true;
            request.Query.Timeout = parameters.Timeout ?? TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
            
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> RemoveProfileBanner(IRemoveProfileBannerParameters parameters, ITwitterRequest request)
        {
            var query = _accountSettingsQueryGenerator.GetRemoveProfileBannerQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest(request);
        }
    }
}