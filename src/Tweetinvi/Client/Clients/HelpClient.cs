using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class HelpClient : IHelpClient
    {
        private readonly IHelpRequester _helpRequester;

        public HelpClient(IHelpRequester helpRequester)
        {
            _helpRequester = helpRequester;
        }

        public Task<ITwitterConfiguration> GetTwitterConfiguration()
        {
            return GetTwitterConfiguration(new GetTwitterConfigurationParameters());
        }

        public async Task<ITwitterConfiguration> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters)
        {
            var twitterResult = await _helpRequester.GetTwitterConfiguration(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<SupportedLanguage[]> GetSupportedLanguages()
        {
            return GetSupportedLanguages(new GetSupportedLanguagesParameters());
        }

        public async Task<SupportedLanguage[]> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters)
        {
            var twitterResult = await _helpRequester.GetSupportedLanguages(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<IPlace> GetPlace(string placeId)
        {
            return GetPlace(new GetPlaceParameters(placeId));
        }

        public async Task<IPlace> GetPlace(IGetPlaceParameters parameters)
        {
            var result = await _helpRequester.GetPlace(parameters).ConfigureAwait(false);
            return result?.DataTransferObject;
        }

        public async Task<IPlace[]> SearchGeo(IGeoSearchParameters parameters)
        {
            var result = await _helpRequester.SearchGeo(parameters).ConfigureAwait(false);
            return result?.DataTransferObject?.Result.Places;
        }

        public Task<IPlace[]> SearchGeoReverse(ICoordinates coordinates)
        {
            return SearchGeoReverse(new GeoSearchReverseParameters(coordinates));
        }

        public async Task<IPlace[]> SearchGeoReverse(IGeoSearchReverseParameters parameters)
        {
            var result = await _helpRequester.SearchGeoReverse(parameters).ConfigureAwait(false);
            return result?.DataTransferObject?.Result.Places;
        }
    }
}