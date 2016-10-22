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
        IMedia WaitForMediaProcessingToGetAllMetadata(IMedia media);
    }

    public class UploadHelper : IUploadHelper
    {
        private readonly IThreadHelper _threadHelper;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;

        public UploadHelper(
            IThreadHelper threadHelper,
            IUploadQueryExecutor uploadQueryExecutor)
        {
            _threadHelper = threadHelper;
            _uploadQueryExecutor = uploadQueryExecutor;
        }

        public IMedia WaitForMediaProcessingToGetAllMetadata(IMedia media)
        {
            var isProcessed = IsMediaProcessed(media.UploadedMediaInfo);
            if (isProcessed)
            {
                return media;
            }

            var processingInfoDelay = media.UploadedMediaInfo.ProcessingInfo.CheckAfterInMilliseconds;
            var dateWhenProcessingCanBeChecked = media.UploadedMediaInfo.CreatedDate.Add(TimeSpan.FromMilliseconds(processingInfoDelay));

            var timeToWait = (int)DateTime.Now.Subtract(dateWhenProcessingCanBeChecked).TotalMilliseconds;

            IUploadedMediaInfo mediaStatus = null;
            while (!isProcessed)
            {
                _threadHelper.Sleep(timeToWait);

                // The second parameter (false) informs Tweetinvi that you are manually awaiting the media to be ready
                mediaStatus = _uploadQueryExecutor.GetMediaStatus(media, false);
                isProcessed = IsMediaProcessed(mediaStatus.ProcessingInfo);
                timeToWait = mediaStatus.ProcessingInfo.CheckAfterInMilliseconds;
            }

            media = media.CloneWithoutUploadInfo();
            media.UploadedMediaInfo = mediaStatus;

            return media;
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
