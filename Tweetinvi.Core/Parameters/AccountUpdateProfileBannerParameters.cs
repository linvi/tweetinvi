using System;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// Parameters used to upload a banner for the user profile.
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/update_profile_banner
    /// </summary>
    public interface IAccountUpdateProfileBannerParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Binary of the banner image.
        /// </summary>
        byte[] Binary { get; set; }

        /// <summary>
        ///The width of the preferred section of the image being uploaded in pixels. Use with height, offset_left, and offset_top to select the desired region of the image to use.
        /// </summary>
        int? Width { get; set; }

        /// <summary>
        /// The height of the preferred section of the image being uploaded in pixels. Use with width, offset_left, and offset_top to select the desired region of the image to use.
        /// </summary>
        int? Height { get; set; }

        /// <summary>
        /// The number of pixels by which to offset the uploaded image from the left. Use with height, width, and offset_top to select the desired region of the image to use.
        /// </summary>
        int? OffsetLeft { get; set; }

        /// <summary>
        /// The number of pixels by which to offset the uploaded image from the top. Use with height, width, and offset_left to select the desired region of the image to use.
        /// </summary>
        int? OffsetTop { get; set; }

        /// <summary>
        /// If set, the http request will use this duration before throwing an exception.
        /// </summary>
        TimeSpan? Timeout { get; set; }
    }

    /// <summary>
    /// Parameters used to upload a banner for the user profile.
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/update_profile_banner
    /// </summary>
    public class AccountUpdateProfileBannerParameters : CustomRequestParameters, IAccountUpdateProfileBannerParameters
    {
        public AccountUpdateProfileBannerParameters(byte[] image)
        {
            Binary = image;
        }

        public byte[] Binary { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? OffsetLeft { get; set; }
        public int? OffsetTop { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}