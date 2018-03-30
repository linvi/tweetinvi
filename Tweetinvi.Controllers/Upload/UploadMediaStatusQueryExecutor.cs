using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadMediaStatusQueryExecutor
    {
        IUploadedMediaInfo GetMediaStatus(IMedia media, bool autoAwait = true);
    }

    public class UploadMediaStatusQueryExecutor : IUploadMediaStatusQueryExecutor
    {
        private readonly IThreadHelper _threadHelper;
        private readonly ITwitterAccessor _twitterAccessor;

        public UploadMediaStatusQueryExecutor(IThreadHelper threadHelper, ITwitterAccessor twitterAccessor)
        {
            _threadHelper = threadHelper;
            _twitterAccessor = twitterAccessor;
        }

        public IUploadedMediaInfo GetMediaStatus(IMedia media, bool autoWait = true)
        {
            if (!media.HasBeenUploaded)
            {
                throw new InvalidOperationException(Resources.Exception_Upload_Status_NotUploaded);
            }

            if (media.UploadedMediaInfo.ProcessingInfo == null)
            {
                throw new InvalidOperationException(Resources.Exception_Upload_Status_No_ProcessingInfo);
            }

            if (autoWait)
            {
                var timeBeforeOperationPermitted = TimeSpan.FromSeconds(media.UploadedMediaInfo.ProcessingInfo.CheckAfterInSeconds);

                var waitTimeRemaining = media.UploadedMediaInfo.CreatedDate.Add(timeBeforeOperationPermitted).Subtract(DateTime.Now);
                if (waitTimeRemaining.TotalMilliseconds > 0)
                {
                    _threadHelper.Sleep((int)waitTimeRemaining.TotalMilliseconds);
                }
            }

            return _twitterAccessor.ExecuteGETQuery<IUploadedMediaInfo>($"https://upload.twitter.com/1.1/media/upload.json?command=STATUS&media_id={media.MediaId}");
        }
    }
}
