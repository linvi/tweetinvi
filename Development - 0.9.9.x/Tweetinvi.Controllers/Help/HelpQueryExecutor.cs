using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpQueryExecutor
    {
        ITokenRateLimits GetCurrentCredentialsRateLimits();
        ITokenRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials);
        string GetTwitterPrivacyPolicy();
        ITwitterConfiguration GetTwitterConfiguration();
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

        public ITokenRateLimits GetCurrentCredentialsRateLimits()
        {
            string query = _helpQueryGenerator.GetCredentialsLimitsQuery();
            return _twitterAccessor.ExecuteGETQuery<ITokenRateLimits>(query);
        }

        public ITokenRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
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
    }
}