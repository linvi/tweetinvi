using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Public.Models.Enum;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

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

        private static readonly IUploadHelper _uploadHelper;

        static Upload()
        {
            Initialize();

            _uploadHelper = TweetinviContainer.Resolve<IUploadHelper>();
        }

        private static void Initialize()
        {
            _uploadQueryExecutor = TweetinviContainer.Resolve<IUploadQueryExecutor>();
        }

        /// <summary>
        /// Upload a collection of media. The media uploaded info is updated.
        /// If the uploaded info is null the media failed to be uploaded.
        /// </summary>
        public static void UploadMedias(IEnumerable<IMedia> medias, bool forceReUpload)
        {
            UploadQueryExecutor.UploadMedias(medias, forceReUpload);
        }

        /// <summary>
        /// Create and Upload a media on upload.twitter.com
        /// </summary>
        public static IMedia UploadBinary(byte[] binary)
        {
            return UploadBinaries(new[] { binary }).FirstOrDefault();
        }

        /// <summary>
        /// Create and Upload multiple medias on upload.twitter.com
        /// </summary>
        public static IEnumerable<IMedia> UploadBinaries(IEnumerable<byte[]> binaries)
        {
            return UploadQueryExecutor.UploadBinaries(binaries);
        }

        /// <summary>
        /// Upload a video to twitter.
        /// </summary>
        public static IMedia UploadImage(byte[] binary)
        {
            return UploadQueryExecutor.UploadBinary(binary);
        }

        /// <summary>
        /// Upload a video to twitter. The mediaCategory needs to be `tweet_video` 
        /// if you want to use GetMediaStatus.
        /// </summary>
        public static IMedia UploadVideo(byte[] binary, string mediaType = "video/mp4", string mediaCategory = null)
        {
            return UploadQueryExecutor.UploadVideo(binary, mediaType, mediaCategory);
        }

        /// <summary>
        /// Upload a video to twitter. The mediaCategory needs to be `tweet_video` 
        /// if you want to use GetMediaStatus.
        /// </summary>
        public static IMedia UploadVideo(byte[] binary, UploadMediaCategory mediaCategory)
        {
            return UploadQueryExecutor.UploadVideo(binary, mediaCategory);
        }

        /// <summary>
        /// Upload a binary using the chunked upload mechanism.
        /// </summary>
        public static IMedia ChunkUploadBinary(byte[] binary, string mediaType, UploadMediaCategory mediaCategory)
        {
            return UploadQueryExecutor.ChunkUploadBinary(binary, mediaType, mediaCategory);
        }

        /// <summary>
        /// Upload a binary using the chunked upload mechanism.
        /// </summary>
        public static IMedia ChunkUploadBinary(byte[] binary, string mediaType)
        {
            return UploadQueryExecutor.ChunkUploadBinary(binary, mediaType);
        }

        /// <summary>
        /// Upload a binary using the chunked upload mechanism.
        /// </summary>
        public static IMedia ChunkUploadBinary(IUploadQueryParameters parameters)
        {
            return UploadQueryExecutor.ChunkUploadBinary(parameters);
        }

        /// <summary>
        /// A chunked uploader is an object that allows developers to 
        /// upload binaries using the chunked upload endpoint.
        /// It is interesting to notice that chunked upload allows to
        /// publish a binary in multiple uploads.
        /// </summary>
        public static IChunkedUploader CreateChunkedUploader()
        {
            return UploadQueryExecutor.CreateChunkedUploader();
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
            return UploadQueryExecutor.GetMediaStatus(media, waitForStatusToBeAvailable);
        }

        /// <summary>
        /// Wait for Twitter to process the uploaded binary and returns a new media object containing all the available metadata.
        /// </summary>
        public static IMedia WaitForMediaProcessingToGetAllMetadata(IMedia media)
        {
            return _uploadHelper.WaitForMediaProcessingToGetAllMetadata(media);
        }
    }
}