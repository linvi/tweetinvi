using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi
{
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

        static Upload()
        {
            Initialize();
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
        /// Upload a video to twitter
        /// </summary>
        public static IMedia UploadImage(byte[] binary)
        {
            return UploadQueryExecutor.UploadBinary(binary);
        }

        /// <summary>
        /// Upload a video to twitter
        /// </summary>
        public static IMedia UploadVideo(byte[] binary, string mediaType = "video/mp4")
        {
            return UploadQueryExecutor.UploadVideo(binary, mediaType);
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
    }
}