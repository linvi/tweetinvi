using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalHelpRequester : IHelpRequester, IBaseRequester
    {
    }

    public class HelpRequester : BaseRequester, IInternalHelpRequester
    {
        private readonly IHelpController _helpController;
        private readonly IHelpClientRequiredParametersValidator _validator;

        public HelpRequester(
            IHelpController helpController,
            IHelpClientRequiredParametersValidator validator)
        {
            _helpController = helpController;
            _validator = validator;
        }

        public Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters)
        {
            var request = CreateRequest();
            _validator.Validate(parameters);

            return ExecuteRequest(() => _helpController.GetRateLimits(parameters, request), request);
        }
    }
}