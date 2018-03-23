using System;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Upload image, gif or videos.
    /// </summary>
    public static class Upload
    {
        [ThreadStatic]
        private static IUploadQueryExecutor _uploadQueryExecutor;

        /// <summary>
        /// Controller handling any Upload request
        /// </summary>
        public static IUploadQueryExecutor UploadQueryExecutor
        {
            get
            {
                if (_uploadQueryExecutor == null)
                {
                    Initialize();
                }

                return _uploadQueryExecutor;
            }
        }

        [ThreadStatic]
        private static IUploadMediaStatusQueryExecutor _uploadMediaStatusQueryExecutor;

        /// <summary>
        /// Controller handling any Upload request
        /// </summary>
        public static IUploadMediaStatusQueryExecutor UploadMediaStatusQueryExecutor
        {
            get
            {
                if (_uploadMediaStatusQueryExecutor == null)
                {
                    Initialize();
                }

                return _uploadMediaStatusQueryExecutor;
            }
        }

        private static readonly IUploadHelper _uploadHelper;

        static Upload()
        {
            Initialize();

            _uploadHelper = TweetinviContainer.Resolve<IUploadHelper>();
        }

        private static void Initialize()
        {
            _uploadQueryExecutor = TweetinviContainer.Resolve<IUploadQueryExecutor>();
            _uploadMediaStatusQueryExecutor = TweetinviContainer.Resolve<IUploadMediaStatusQueryExecutor>();
        }

        /// <summary>
        /// Upload a media on upload.twitter.com
        /// </summary>
        public static IMedia UploadBinary(IUploadParameters parameters)
        {
            return UploadQueryExecutor.UploadBinary(parameters);
        }

        /// <summary>
        /// Upload a media on upload.twitter.com
        /// </summary>
        public static IMedia UploadBinary(byte[] binary, IUploadOptionalParameters optionalParameters = null)
        {
            return UploadQueryExecutor.UploadBinary(binary, optionalParameters);
        }

        /// <summary>
        /// Upload a video to twitter. The mediaCategory needs to be `tweet_video` 
        /// if you want to use GetMediaStatus.
        /// </summary>
        public static IMedia UploadVideo(byte[] binary, IUploadVideoOptionalParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = new UploadVideoOptionalParameters();
            }

            return UploadQueryExecutor.UploadBinary(binary, parameters);
        }

        /// <summary>
        /// Upload a video to twitter. The mediaCategory needs to be `tweet_video` 
        /// if you want to use GetMediaStatus.
        /// </summary>
        public static IMedia UploadVideo(IUploadVideoParameters parameters)
        {
            return UploadQueryExecutor.UploadBinary(parameters);
        }

        /// <summary>
        /// Add metadata to a media that has been uploaded.
        /// </summary>
        public static bool AddMediaMetadata(IMediaMetadata metadata)
        {
            return UploadQueryExecutor.AddMediaMetadata(metadata);
        }

        /// <summary>
        /// Get the status of the media. NOTE that this is only available if the `tweet_video` media category
        /// has been set. And the endpoint is available only after the 
        /// UploadedMediaInfo.ProcessingInfo.CheckAfterInSeconds Timespan has completed.
        /// </summary>
        public static IUploadedMediaInfo GetMediaStatus(IMedia media, bool waitForStatusToBeAvailable = true)
        {
            return UploadMediaStatusQueryExecutor.GetMediaStatus(media, waitForStatusToBeAvailable);
        }

        /// <summary>
        /// Wait for Twitter to process the uploaded binary and returns a new media object containing all the available metadata.
        /// </summary>
        public static void WaitForMediaToBecomeAvailable(IMedia media)
        {
            _uploadHelper.WaitForMediaProcessingToGetAllMetadata(media);
        }
    }
}