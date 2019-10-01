using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    /// <summary>
    /// A client providing all the actions relative to the account settings
    /// The results from this client contain additional metadata.
    /// </summary>
    public interface IAccountSettingsRequester
    {
        /// <summary>
        /// Update the profile image of the account 
        /// <para>https://dev.twitter.com/rest/reference/post/account/update_profile_image</para>
        /// </summary>
        /// <returns>Twitter result containing the updated user</returns>
        Task<ITwitterResult<IUserDTO>> UpdateProfileImage(IUpdateProfileImageParameters parameters);

        /// <summary>
        /// Update the profile banner of the account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-update_profile_banner </para> 
        /// </summary>
        /// <returns>Twitter result</returns>
        Task<ITwitterResult> UpdateProfileBanner(IUpdateProfileBannerParameters parameters);
        
        /// <summary>
        /// Remove the profile banner of the account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-remove_profile_banner </para> 
        /// </summary>
        /// <returns>Twitter result</returns>
        Task<ITwitterResult> RemoveProfileBanner(IRemoveProfileBannerParameters parameters);
    }
}