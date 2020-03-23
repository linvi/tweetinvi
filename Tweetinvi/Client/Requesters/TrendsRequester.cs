using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client.Requesters
{
    public class TrendsRequester : BaseRequester, ITrendsRequester
    {
        private readonly ITrendsController _trendsController;
        private readonly ITrendsClientRequiredParametersValidator _trendsClientRequiredParametersValidator;

        public TrendsRequester(
            ITrendsController trendsController,
            ITrendsClientRequiredParametersValidator trendsClientRequiredParametersValidator,
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents)
            : base(client, twitterClientEvents)
        {
            _trendsController = trendsController;
            _trendsClientRequiredParametersValidator = trendsClientRequiredParametersValidator;
        }

        public Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAt(IGetTrendsAtParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _trendsController.GetPlaceTrendsAt(parameters, request));
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendLocations(IGetTrendsLocationParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _trendsController.GetTrendLocations(parameters, request));
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _trendsController.GetTrendsLocationCloseTo(parameters, request));
        }
    }
}