using System;
using Tweetinvi.Core.Public.Parameters.Enum;

namespace Tweetinvi.Core.Public.Events
{
    public interface IUploadProgressChanged
    {
        /// <summary>
        /// Type of operation executed for the upload
        /// </summary>
        UploadProgressState State { get; }

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

    public class UploadStateChangedEventArgs : EventArgs, IUploadProgressChanged
    {
        public UploadStateChangedEventArgs(UploadProgressState state, long numberOfBytesUploaded, long totalOfBytesToUpload)
        {
            State = state;
            NumberOfBytesUploaded = numberOfBytesUploaded;
            TotalOfBytesToUpload = totalOfBytesToUpload;
        }

        public UploadProgressState State { get; }

        public long NumberOfBytesUploaded { get; }

        public long TotalOfBytesToUpload { get; }

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
