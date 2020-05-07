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

        public Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _trendsController.GetPlaceTrendsAtAsync(parameters, request));
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _trendsController.GetTrendLocationsAsync(parameters, request));
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters)
        {
            _trendsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _trendsController.GetTrendsLocationCloseToAsync(parameters, request));
        }
    }
}