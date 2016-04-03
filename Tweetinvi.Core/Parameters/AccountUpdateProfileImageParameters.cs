using System;
using System.Runtime.CompilerServices;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// Parameters used to upload the user image.
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/update_profile_image
    /// </summary>
    public interface IAccountUpdateProfileImageParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Binary of the banner image.
        /// </summary>
        byte[] Binary { get; set; }

        /// <summary>
        /// Include tweet entities.
        /// </summary>
        bool IncludeEntities { get; set; }


        /// <summary>
        /// When set to true, statuses will not be included in the returned user objects.
        /// </summary>
        bool SkipStatus { get; set; }

        /// <summary>
        /// If set, the http request will use this duration before throwing an exception.
        /// </summary>
        TimeSpan? Timeout { get; set; }
    }

    /// <summary>
    /// Parameters used to upload a banner for the user profile.
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/update_profile_banner
    /// </summary>
    public class AccountUpdateProfileImageParameters : CustomRequestParameters, IAccountUpdateProfileImageParameters
    {
        public AccountUpdateProfileImageParameters(byte[] image)
        {
            Binary = image;
        }

        public byte[] Binary { get; set; }
        public bool IncludeEntities { get; set; }
        public bool SkipStatus { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}