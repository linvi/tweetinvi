namespace Tweetinvi.Core.QueryGenerators
{
    public interface IHelpQueryGenerator
    {
        string GetCredentialsLimitsQuery();
        string GetTwitterPrivacyPolicyQuery();
        string GetTwitterConfigurationQuery();
        string GetTermsOfServiceQuery();
    }
}