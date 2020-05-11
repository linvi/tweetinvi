using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Events
{
    public interface IMediaUploadProgressChangedEventArgs : IUploadProgressChanged
    {
        /// <summary>
        /// Type of operation executed for the upload
        /// </summary>
        UploadProgressState State { get; }
    }

    /// <summary>
    /// Event that indicates a progress change during a media upload
    /// </summary>
    public class MediaUploadProgressChangedEventArgs : UploadProgressChangedEventArgs, IMediaUploadProgressChangedEventArgs
    {
        public MediaUploadProgressChangedEventArgs(UploadProgressState state, long numberOfBytesUploaded, long totalOfBytesToUpload) : base(numberOfBytesUploaded, totalOfBytesToUpload)
        {
            State = state;
            NumberOfBytesUploaded = numberOfBytesUploaded;
            TotalOfBytesToUpload = totalOfBytesToUpload;
        }

        public UploadProgressState State { get; }

    }
}