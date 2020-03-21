using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IHelpController
    {
        Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IPlace>> GetPlace(IGetPlaceParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeo(IGeoSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverse(IGeoSearchReverseParameters parameters, ITwitterRequest request);
    }
}