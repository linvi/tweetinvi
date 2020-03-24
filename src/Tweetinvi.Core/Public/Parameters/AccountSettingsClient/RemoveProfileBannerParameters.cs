namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-remove_profile_banner
    /// </summary>
    public interface IRemoveProfileBannerParameters : ICustomRequestParameters
    {
    }
    
    /// <inheritdoc/>
    public class RemoveProfileBannerParameters : CustomRequestParameters, IRemoveProfileBannerParameters
    {
    }
}