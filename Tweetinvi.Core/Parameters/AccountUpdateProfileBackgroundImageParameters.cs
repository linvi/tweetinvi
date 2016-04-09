using System;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Parameters
{
    public interface IAccountUpdateProfileBackgroundImageParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Binary of the profile's background image.
        /// Must be a valid GIF, JPG, or PNG image of less than 800 kilobytes in size. Images with width larger than 2048 pixels will be forcibly scaled down.
        /// </summary>
        byte[] Binary { get; }

        /// <summary>
        /// Specify the media to use as the background image.
        /// You will receive a MediaId when uploading via the Upload static class.
        /// </summary>
        long? MediaId { get; }

        /// <summary>
        /// Whether or not to tile the background image.
        /// </summary>
        bool? UseTileMode { get; set; }

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

    public class AccountUpdateProfileBackgroundImageParameters : CustomRequestParameters, IAccountUpdateProfileBackgroundImageParameters
    {
        public AccountUpdateProfileBackgroundImageParameters(long mediaId)
        {
            MediaId = mediaId;
            IncludeEntities = true;
        }

        public AccountUpdateProfileBackgroundImageParameters(IMedia media)
        {
            MediaId = media.MediaId;
            IncludeEntities = true;
        }

        public AccountUpdateProfileBackgroundImageParameters(byte[] imageBinary)
        {
            Binary = imageBinary;
            IncludeEntities = true;
        }

        public byte[] Binary { get; }
        public long? MediaId { get; }
        public bool? UseTileMode { get; set; }
        public bool IncludeEntities { get; set; }
        public bool SkipStatus { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}