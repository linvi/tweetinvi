using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpQueryExecutor
    {
        Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IPlace>> GetPlace(IGetPlaceParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeo(IGeoSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverse(IGeoSearchReverseParameters parameters, ITwitterRequest request);
    }

    public class HelpQueryExecutor : IHelpQueryExecutor
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public HelpQueryExecutor(IHelpQueryGenerator helpQueryGenerator, ITwitterAccessor twitterAccessor)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetRateLimitsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<CredentialsRateLimitsDTO>(request);
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetTwitterConfigurationQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterConfiguration>(request);
        }

        public Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetSupportedLanguagesQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<SupportedLanguage[]>(request);
        }

        public Task<ITwitterResult<IPlace>> GetPlace(IGetPlaceParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetPlaceQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IPlace>(request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeo(IGeoSearchParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetSearchGeoQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<SearchGeoSearchResultDTO>(request);
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverse(IGeoSearchReverseParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetSearchGeoReverseQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<SearchGeoSearchResultDTO>(request);
        }
    }
}