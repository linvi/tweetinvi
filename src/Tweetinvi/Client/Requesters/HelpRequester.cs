using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class HelpRequester : BaseRequester, IHelpRequester
    {
        private readonly IHelpController _helpController;
        private readonly IHelpClientRequiredParametersValidator _validator;

        public HelpRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IHelpController helpController,
            IHelpClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _helpController = helpController;
            _validator = validator;
        }

        public Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimitsAsync(IGetRateLimitsParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequestAsync(request =>
            {
                if (parameters.TrackerMode != null)
                {
                    request.ExecutionContext.RateLimitTrackerMode = parameters.TrackerMode.Value;
                }

                return _helpController.GetRateLimitsAsync(parameters, request);
            });
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _helpController.GetTwitterConfigurationAsync(parameters, request));
        }

        public Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _helpController.GetSupportedLanguagesAsync(parameters, request));
        }

        public Task<ITwitterResult<IPlace>> GetPlaceAsync(IGetPlaceParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _helpController.GetPlaceAsync(parameters, request));
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoAsync(IGeoSearchParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _helpController.SearchGeoAsync(parameters, request));
        }

        public Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _helpController.SearchGeoReverseAsync(parameters, request));
        }
    }
}