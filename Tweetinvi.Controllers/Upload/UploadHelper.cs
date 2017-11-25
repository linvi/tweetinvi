using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Public.Models.Enum;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadHelper
    {
        void WaitForMediaProcessingToGetAllMetadata(IMedia media);
    }

    public class UploadHelper : IUploadHelper
    {
        private readonly IThreadHelper _threadHelper;
        private readonly IUploadMediaStatusQueryExecutor _uploadQueryExecutor;

        public UploadHelper(
            IThreadHelper threadHelper,
            IUploadMediaStatusQueryExecutor uploadQueryExecutor)
        {
            _threadHelper = threadHelper;
            _uploadQueryExecutor = uploadQueryExecutor;
        }

        public void WaitForMediaProcessingToGetAllMetadata(IMedia media)
        {
            if (media == null)
            {
                return;
            }

            var isProcessed = IsMediaProcessed(media.UploadedMediaInfo);
            if (isProcessed)
            {
                return;
            }

            var processingInfoDelay = media.UploadedMediaInfo.ProcessingInfo.CheckAfterInMilliseconds;
            var dateWhenProcessingCanBeChecked = media.UploadedMediaInfo.CreatedDate.Add(TimeSpan.FromMilliseconds(processingInfoDelay));

            var timeToWait = (int)dateWhenProcessingCanBeChecked.Subtract(DateTime.Now).TotalMilliseconds;

            IUploadedMediaInfo mediaStatus = null;
            while (!isProcessed)
            {
                _threadHelper.Sleep(timeToWait);

                // The second parameter (false) informs Tweetinvi that you are manually awaiting the media to be ready
                mediaStatus = _uploadQueryExecutor.GetMediaStatus(media, false);
                isProcessed = IsMediaProcessed(mediaStatus.ProcessingInfo);
                timeToWait = mediaStatus.ProcessingInfo.CheckAfterInMilliseconds;
            }

            media.UploadedMediaInfo = mediaStatus;
        }

        private bool IsMediaProcessed(IUploadedMediaInfo mediaInfo)
        {
            return IsMediaProcessed(mediaInfo?.ProcessingInfo);
        }

        private bool IsMediaProcessed(IUploadProcessingInfo processInfo)
        {
            var state = processInfo?.ProcessingState;

            if (processInfo == null)
            {
                throw new InvalidOperationException(Resources.Exception_Upload_Status_No_ProcessingInfo);
            }

            return state == ProcessingState.Succeeded || state == ProcessingState.Failed;
        }
    }
}
