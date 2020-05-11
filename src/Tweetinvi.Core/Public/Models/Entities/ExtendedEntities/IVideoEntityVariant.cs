namespace Tweetinvi.Models.Entities.ExtendedEntities
{
    public interface IVideoEntityVariant
    {
        /// <summary>
        /// Video bitrate in bits-per-second
        /// </summary>
        int Bitrate { get; set; }

        /// <summary>
        /// MIME type of the video
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Direct URL for the video variant
        /// </summary>
        string URL { get; set; }
    }
}