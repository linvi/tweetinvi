using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpQueryExecutor
    {
        ICredentialsRateLimits GetCurrentCredentialsRateLimits();
        ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials);
        string GetTwitterPrivacyPolicy();
        ITwitterConfiguration GetTwitterConfiguration();
        string GetTermsOfService();
    }

    public class HelpQueryExecutor : IHelpQueryExecutor
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public HelpQueryExecutor(
            IHelpQueryGenerator helpQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ICredentialsAccessor credentialsAccessor)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _credentialsAccessor = credentialsAccessor;
        }

        public ICredentialsRateLimits GetCurrentCredentialsRateLimits()
        {
            string query = _helpQueryGenerator.GetCredentialsLimitsQuery();
            return _twitterAccessor.ExecuteGETQuery<ICredentialsRateLimits>(query);
        }

        public ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var savedCredentials = _credentialsAccessor.CurrentThreadCredentials;
            _credentialsAccessor.CurrentThreadCredentials = credentials;
            var rateLimits = GetCurrentCredentialsRateLimits();
            _credentialsAccessor.CurrentThreadCredentials = savedCredentials;
            return rateLimits;
        }

        public string GetTwitterPrivacyPolicy()
        {
            string query = _helpQueryGenerator.GetTwitterPrivacyPolicyQuery();
            var privacyJson = _twitterAccessor.ExecuteGETQuery(query);

            if (privacyJson == null)
            {
                return null;
            }

            return privacyJson["privacy"].ToObject<string>();
        }

        public ITwitterConfiguration GetTwitterConfiguration()
        {
            string query = _helpQueryGenerator.GetTwitterConfigurationQuery();
            return _twitterAccessor.ExecuteGETQuery<ITwitterConfiguration>(query);
        }

        public string GetTermsOfService()
        {
            var query = _helpQueryGenerator.GetTermsOfServiceQuery();
            return _twitterAccessor.ExecuteGETQueryWithPath<string>(query, "tos");
        }
    }
}