using System.Threading.Tasks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the actions relative to the account settings
    /// </summary>
    public interface IAccountSettingsClient
    {
        /// <summary>
        /// Update the profile image of the account
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/account/update_profile_image</para> 
        /// </summary>
        /// <returns>Whether the update page profile operation was successful</returns>
        Task<bool> UpdateProfileImage(IUpdateProfileImageParameters parameters);
    }
}