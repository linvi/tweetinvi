using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpJsonController
    {
        string GetTokenRateLimits();
        string GetTwitterPrivacyPolicy();
    }

    public class HelpJsonController : IHelpJsonController
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public HelpJsonController(
            IHelpQueryGenerator helpQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public string GetTokenRateLimits()
        {
            string query = _helpQueryGenerator.GetCredentialsLimitsQuery();
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetTwitterPrivacyPolicy()
        {
            string query = _helpQueryGenerator.GetTwitterPrivacyPolicyQuery();
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }
    }
}