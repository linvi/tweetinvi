using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.QueryGenerators;

namespace Tweetinvi.Controllers.Help
{
    public class HelpQueryGenerator : IHelpQueryGenerator
    {
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