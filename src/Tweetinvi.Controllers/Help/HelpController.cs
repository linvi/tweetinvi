using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Help
{
    public class HelpController : IHelpController
    {
        private readonly IHelpQueryExecutor _helpQueryExecutor;

        public HelpController(IHelpQueryExecutor helpQueryExecutor)
        {
            _helpQueryExecutor = helpQueryExecutor;
        }

        public Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimitsAsync(IGetRateLimitsParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetRateLimitsAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetTwitterConfigurationAsync(parameters, request);
        }

        public Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetSupportedLanguagesAsync(parameters, request);
        }

        public Task<ITwitterResult<IPlace>> GetPlaceAsync(IGetPlaceParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetPlaceAsync(parameters, request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoAsync(IGeoSearchParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.SearchGeoAsync(parameters, request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.SearchGeoReverseAsync(parameters, request);
        }
    }
}