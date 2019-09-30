using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsQueryExecutor
    {
        Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters, ITwitterRequest request);
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
    }
}