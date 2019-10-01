using Tweetinvi.Core.Public.Parameters.Enum;

namespace Tweetinvi.Events
{
    public interface IMediaUploadProgressChangedEventArgs : IUploadProgressChanged
    {
        /// <summary>
        /// Type of operation executed for the upload
        /// </summary>
        UploadProgressState State { get; }
    }
    
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