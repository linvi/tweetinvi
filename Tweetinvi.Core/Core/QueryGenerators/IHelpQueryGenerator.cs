using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IHelpQueryGenerator
    {
        string GetRateLimitsQuery(IGetRateLimitsParameters parameters);
        string GetCredentialsLimitsQuery();
        string GetTwitterPrivacyPolicyQuery();
        string GetTwitterConfigurationQuery();
        string GetTermsOfServiceQuery();
    }
}