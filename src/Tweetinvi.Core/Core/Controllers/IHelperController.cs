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
        Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimitsAsync(IGetRateLimitsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IPlace>> GetPlaceAsync(IGetPlaceParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoAsync(IGeoSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters, ITwitterRequest request);
    }
}