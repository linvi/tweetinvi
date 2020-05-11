using System;

namespace Tweetinvi.Events
{
    public interface IUploadProgressChanged
    {
        /// <summary>
        /// Numbers of bytes that have been successfully sent at the current time
        /// </summary>
        long NumberOfBytesUploaded { get; }

        /// <summary>
        /// Total number of bytes of the upload
        /// </summary>
        long TotalOfBytesToUpload { get; }

        /// <summary>
        /// Percentage of completion of the upload
        /// </summary>
        int Percentage { get; }
    }

    /// <summary>
    /// Event that indicates a progress change during an upload
    /// </summary>
    public class UploadProgressChangedEventArgs : EventArgs, IUploadProgressChanged
    {
        public UploadProgressChangedEventArgs(long numberOfBytesUploaded, long totalOfBytesToUpload)
        {
            NumberOfBytesUploaded = numberOfBytesUploaded;
            TotalOfBytesToUpload = totalOfBytesToUpload;
        }

        public long NumberOfBytesUploaded { get; protected set; }
        public long TotalOfBytesToUpload { get; protected set; }
        public int Percentage
        {
            get
            {
                if (TotalOfBytesToUpload == 0)
                {
                    return 0;
                }

                return (int)((float)NumberOfBytesUploaded / TotalOfBytesToUpload * 100);
            }
        }
    }
}
