using System;
using Tweetinvi.Core.Public.Parameters.Enum;

namespace Tweetinvi.Core.Public.Events
{
    public interface IUploadProgressChanged
    {
        UploadProgressState State { get; }
        int NumberOfBytesUploaded { get; }
        int TotalOfBytesToUpload { get; }
        int Percentage { get; }
    }

    public class UploadStateChangedEventArgs : EventArgs, IUploadProgressChanged
    {

        public UploadStateChangedEventArgs(UploadProgressState state, int numberOfBytesUploaded, int totalOfBytesToUpload)
        {
            State = state;
            NumberOfBytesUploaded = numberOfBytesUploaded;
            TotalOfBytesToUpload = totalOfBytesToUpload;
        }

        public UploadProgressState State { get; }

        public int NumberOfBytesUploaded { get; }

        public int TotalOfBytesToUpload { get; }

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
