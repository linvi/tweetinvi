using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
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

        public Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(request =>
            {
                if (parameters.TrackerMode != null)
                {
                    request.ExecutionContext.RateLimitTrackerMode = parameters.TrackerMode.Value;
                }

                return _helpController.GetRateLimits(parameters, request);
            });
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _helpController.GetTwitterConfiguration(parameters, request));
        }

        public Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguages(IGetSupportedLanguagesParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _helpController.GetSupportedLanguages(parameters, request));
        }
    }
}