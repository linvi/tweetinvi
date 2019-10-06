namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-update_profile
    /// </summary>
    public interface IUpdateProfileParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Full name associated with the profile. Maximum of 50 characters.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// URL associated with the profile. Will be prepended with “http://” if not present. Maximum of 100 characters.
        /// </summary>
        string WebsiteUrl { get; set; }

        /// <summary>
        /// The city or country describing where the user of the account is located. The contents are not normalized or geocoded in any way.
        /// Maximum of 30 characters.
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// A description/bio of the user owning the account. Maximum of 160 characters.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Sets a hex value that controls the color scheme of links used on the authenticating user’s profile page on twitter.com. 
        /// This must be a valid hexadecimal value, and may be either three or six characters (ex: F00 or FF0000).
        /// </summary>
        string ProfileLinkColor { get; set; }

        /// <summary>
        /// The entities node will not be included when set to false.
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// When set to true, statuses will not be included in the returned user objects.
        /// </summary>
        bool? SkipStatus { get; set; }
    }

    /// <inheritdoc/>
    public class UpdateProfileParameters : CustomRequestParameters, IUpdateProfileParameters
    {
        public UpdateProfileParameters()
        {
            IncludeEntities = true;
            SkipStatus = false;
        }

        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Description { get; set; }
        /// <inheritdoc/>
        public string Location { get; set; }
        /// <inheritdoc/>
        public string WebsiteUrl { get; set; }

        /// <inheritdoc/>
        public string ProfileLinkColor { get; set; }
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc/>
        public bool? SkipStatus { get; set; }
    }
}