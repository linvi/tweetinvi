using System.Threading.Tasks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the actions relative to the account settings
    /// </summary>
    public interface IAccountSettingsClient
    {
        /// <inheritdoc cref="UpdateProfileImage(IUpdateProfileImageParameters)" />
        Task<bool> UpdateProfileImage(byte[] binary);

        /// <summary>
        /// Update the profile image of the account
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/account/update_profile_image</para> 
        /// </summary>
        /// <returns>Whether the update of the profile image operation was successful</returns>
        Task<bool> UpdateProfileImage(IUpdateProfileImageParameters parameters);

        /// <inheritdoc cref="UpdateProfileBanner(IUpdateProfileBannerParameters)" />
        Task<bool> UpdateProfileBanner(byte[] binary);
        
        /// <summary>
        /// Update the profile banner of the account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-update_profile_banner </para> 
        /// </summary>
        /// <returns>Whether the update of the profile banner operation was successful</returns>
        Task<bool> UpdateProfileBanner(IUpdateProfileBannerParameters parameters);
    }
}