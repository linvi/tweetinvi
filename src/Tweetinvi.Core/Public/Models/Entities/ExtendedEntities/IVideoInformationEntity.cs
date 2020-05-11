namespace Tweetinvi.Models.Entities.ExtendedEntities
{
    public interface IVideoInformationEntity
    {
        /// <summary>
        /// Video aspect ratio (width, height)
        /// </summary>
        int[] AspectRatio { get; set; }

        /// <summary>
        /// Duration of video in milliseconds
        /// </summary>
        int DurationInMilliseconds { get; set; }

        /// <summary>
        /// Video variants for different codecs, bitrates, etc.
        /// </summary>
        IVideoEntityVariant[] Variants { get; set; }
    }
}