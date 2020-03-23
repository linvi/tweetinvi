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

        public Task<IGetTrendsAtResult> GetPlaceTrendsAt(long woeid)
        {
            return GetPlaceTrendsAt(new GetTrendsAtParameters(woeid));
        }

        public async Task<IGetTrendsAtResult> GetPlaceTrendsAt(IGetTrendsAtParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetPlaceTrendsAt(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject[0];
        }

        public Task<ITrendLocation[]> GetTrendLocations()
        {
            return GetTrendLocations(new GetTrendsLocationParameters());
        }

        public async Task<ITrendLocation[]> GetTrendLocations(IGetTrendsLocationParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetTrendLocations(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<ITrendLocation[]> GetTrendsLocationCloseTo(double latitude, double longitude)
        {
            return GetTrendsLocationCloseTo(new GetTrendsLocationCloseToParameters(latitude, longitude));
        }

        public Task<ITrendLocation[]> GetTrendsLocationCloseTo(ICoordinates coordinates)
        {
            return GetTrendsLocationCloseTo(new GetTrendsLocationCloseToParameters(coordinates));
        }

        public async Task<ITrendLocation[]> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters)
        {
            var twitterResult = await _client.Raw.Trends.GetTrendsLocationCloseTo(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}