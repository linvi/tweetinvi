using System;
using Tweetinvi.Events;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/rest/reference/post/account/update_profile_image
    /// </summary>
    public interface IUpdateProfileImageParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Binary of the profile image. 
        /// Must be a valid GIF, JPG, or PNG image of less than 700 kilobytes in size. Images with width larger than 400 pixels will be scaled down.
        /// </summary>
        byte[] Binary { get; set; }

        /// <summary>
        /// Include tweet entities.
        /// </summary>
        bool? IncludeEntities { get; set; }


        /// <summary>
        /// When set to true, statuses will not be included in the returned user objects.
        /// </summary>
        bool? SkipStatus { get; set; }

        /// <summary>
        /// If set, the http request will use this duration before throwing an exception.
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Action invoked to show the progress of the upload. {current / total}
        /// </summary>
        Action<IUploadProgressChanged> UploadProgressChanged { get; set; }
    }

    /// <inheritdoc/>
    public class UpdateProfileImageParameters : CustomRequestParameters, IUpdateProfileImageParameters
    {
        public UpdateProfileImageParameters(byte[] image)
        {
            Binary = image;
        }

        /// <inheritdoc/>
        public byte[] Binary { get; set; }
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc/>
        public bool? SkipStatus { get; set; }
        /// <inheritdoc/>
        public TimeSpan? Timeout { get; set; }
        /// <inheritdoc/>
        public Action<IUploadProgressChanged> UploadProgressChanged { get; set; }
    }
}