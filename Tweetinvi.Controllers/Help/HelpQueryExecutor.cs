using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpQueryExecutor
    {
        Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits();
        Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials);
        Task<string> GetTwitterPrivacyPolicy();
        Task<ITwitterConfiguration> GetTwitterConfiguration();
        Task<string> GetTermsOfService();
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

        public Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits()
        {
            string query = _helpQueryGenerator.GetCredentialsLimitsQuery();
            return _twitterAccessor.ExecuteGETQuery<ICredentialsRateLimits>(query);
        }

        public Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var savedCredentials = _credentialsAccessor.CurrentThreadCredentials;
            _credentialsAccessor.CurrentThreadCredentials = credentials;
            var rateLimits = GetCurrentCredentialsRateLimits();
            _credentialsAccessor.CurrentThreadCredentials = savedCredentials;
            return rateLimits;
        }

        public async Task<string> GetTwitterPrivacyPolicy()
        {
            string query = _helpQueryGenerator.GetTwitterPrivacyPolicyQuery();
            var privacyJson = await _twitterAccessor.ExecuteGETQuery(query);

            if (privacyJson == null)
            {
                return null;
            }

            return privacyJson["privacy"].ToObject<string>();
        }

        public Task<ITwitterConfiguration> GetTwitterConfiguration()
        {
            string query = _helpQueryGenerator.GetTwitterConfigurationQuery();
            return _twitterAccessor.ExecuteGETQuery<ITwitterConfiguration>(query);
        }

        public Task<string> GetTermsOfService()
        {
            var query = _helpQueryGenerator.GetTermsOfServiceQuery();
            return _twitterAccessor.ExecuteGETQueryWithPath<string>(query, "tos");
        }
    }
}