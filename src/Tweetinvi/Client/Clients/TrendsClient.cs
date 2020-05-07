using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client
{
    public class TrendsClient : ITrendsClient
    {
        private readonly ITwitterClient _client;

        public TrendsClient(ITwitterClient client)
        {
            _client = client;
        }

        public ITrendsClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<IGetTrendsAtResult> GetPlaceTrendsAtAsync(long woeid)
        {
            return GetPlaceTrendsAtAsync(new GetTrendsAtParameters(woeid));
        }

        public async Task<IGetTrendsAtResult> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetPlaceTrendsAtAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject[0];
        }

        public Task<ITrendLocation[]> GetTrendLocationsAsync()
        {
            return GetTrendLocationsAsync(new GetTrendsLocationParameters());
        }

        public async Task<ITrendLocation[]> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetTrendLocationsAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(double latitude, double longitude)
        {
            return GetTrendsLocationCloseToAsync(new GetTrendsLocationCloseToParameters(latitude, longitude));
        }

        public Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(ICoordinates coordinates)
        {
            return GetTrendsLocationCloseToAsync(new GetTrendsLocationCloseToParameters(coordinates));
        }

        public async Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetTrendsLocationCloseToAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}