namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IHelpQueryGenerator
    {
        string GetCredentialsLimitsQuery();
        string GetTwitterPrivacyPolicyQuery();
        string GetTwitterConfigurationQuery();
        string GetTermsOfServiceQuery();
    }
}