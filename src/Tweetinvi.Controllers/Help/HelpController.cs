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

        public Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetRateLimits(parameters, request);
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetTwitterConfiguration(parameters, request);
        }

        public Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetSupportedLanguages(parameters, request);
        }

        public Task<ITwitterResult<IPlace>> GetPlace(IGetPlaceParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetPlace(parameters, request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeo(IGeoSearchParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.SearchGeo(parameters, request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverse(IGeoSearchReverseParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.SearchGeoReverse(parameters, request);
        }
    }
}