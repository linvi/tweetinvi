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

        public Task<ITwitterConfiguration> GetTwitterConfigurationAsync()
        {
            return GetTwitterConfigurationAsync(new GetTwitterConfigurationParameters());
        }

        public async Task<ITwitterConfiguration> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters)
        {
            var twitterResult = await _helpRequester.GetTwitterConfigurationAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<SupportedLanguage[]> GetSupportedLanguagesAsync()
        {
            return GetSupportedLanguagesAsync(new GetSupportedLanguagesParameters());
        }

        public async Task<SupportedLanguage[]> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters)
        {
            var twitterResult = await _helpRequester.GetSupportedLanguagesAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<IPlace> GetPlaceAsync(string placeId)
        {
            return GetPlaceAsync(new GetPlaceParameters(placeId));
        }

        public async Task<IPlace> GetPlaceAsync(IGetPlaceParameters parameters)
        {
            var result = await _helpRequester.GetPlaceAsync(parameters).ConfigureAwait(false);
            return result?.Model;
        }

        public async Task<IPlace[]> SearchGeoAsync(IGeoSearchParameters parameters)
        {
            var result = await _helpRequester.SearchGeoAsync(parameters).ConfigureAwait(false);
            return result?.Model?.Result.Places;
        }

        public Task<IPlace[]> SearchGeoReverseAsync(ICoordinates coordinates)
        {
            return SearchGeoReverseAsync(new GeoSearchReverseParameters(coordinates));
        }

        public async Task<IPlace[]> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters)
        {
            var result = await _helpRequester.SearchGeoReverseAsync(parameters).ConfigureAwait(false);
            return result?.Model?.Result.Places;
        }
    }
}