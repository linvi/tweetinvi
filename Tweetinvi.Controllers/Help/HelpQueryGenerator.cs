using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Controllers.Help
{
    public class HelpQueryGenerator : IHelpQueryGenerator
    {
        public string GetRateLimitsQuery(IGetRateLimitsParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetRateLimit);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetCredentialsLimitsQuery()
        {
            return Resources.Help_GetRateLimit;
        }

        public string GetTwitterPrivacyPolicyQuery()
        {
            return Resources.Help_GetTwitterPrivacyPolicy;
        }

        public string GetTwitterConfigurationQuery()
        {
            return Resources.Help_GetTwitterConfiguration;
        }

        public string GetTermsOfServiceQuery()
        {
            return Resources.Help_GetTermsOfService;
        }
    }
}