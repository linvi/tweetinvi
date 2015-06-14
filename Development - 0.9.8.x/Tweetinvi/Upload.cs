using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi
{
    public static class Upload
    {
        [ThreadStatic]
        private static IUploadQueryExecutor _uploadQueryExecutor;
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
            _uploadQueryExecutor.UploadMedias(medias, forceReUpload);
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
            return _uploadQueryExecutor.UploadBinaries(binaries);
        }
    }
}