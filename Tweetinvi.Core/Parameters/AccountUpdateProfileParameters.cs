using System;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/account/update_profile
    /// </summary>
    public interface IAccountUpdateProfileParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Full name associated with the profile. Maximum of 20 characters.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// URL associated with the profile. Will be prepended with “http://” if not present. Maximum of 100 characters.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The city or country describing where the user of the account is located. The contents are not normalized or geocoded in any way.
        /// Maximum of 30 characters.
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// A description of the user owning the account. Maximum of 160 characters.
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
        bool IncludeEntities { get; set; }

        /// <summary>
        /// When set to true, statuses will not be included in the returned user objects.
        /// </summary>
        bool SkipStatus { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/account/update_profile
    /// </summary>
    public class AccountUpdateProfileParameters : CustomRequestParameters, IAccountUpdateProfileParameters
    {
        public AccountUpdateProfileParameters()
        {
            IncludeEntities = true;
            SkipStatus = false;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 20)
                {
                    throw new ArgumentException("Name cannot contain more than 20 characters.");
                }

                _name = value;
            }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentException("URL cannot contain more thatn 100 characters.");
                }

                _url = value;
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (value != null && value.Length > 30)
                {
                    throw new ArgumentException("Location cannot contain more thatn 100 characters.");
                }

                _location = value;
            }
        }


        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > 160)
                {
                    throw new ArgumentException("Description cannot contain more thatn 100 characters.");
                }

                _description = value;
            }
        }

        public string ProfileLinkColor { get; set; }
        public bool IncludeEntities { get; set; }
        public bool SkipStatus { get; set; }
    }
}