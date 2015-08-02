using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadQueryExecutor
    {
        /// <summary>
        /// Upload a collection of media. The media uploaded info is updated.
        /// If the uploaded info is null the media failed to be uploaded.
        /// </summary>
        void UploadMedias(IEnumerable<IMedia> medias, bool forceReUpload);

        /// <summary>
        /// Upload a single binary to twitter
        /// </summary>
        IMedia UploadBinary(byte[] binary);

        /// <summary>
        /// Create and Upload a media on upload.twitter.com
        /// </summary>
        IEnumerable<IMedia> UploadBinaries(IEnumerable<byte[]> binaries);

        /// <summary>
        /// Create a chunked uploader that give developers access to Twitter chunked uploads
        /// </summary>
        IChunkedUploader CreateChunkedUploader();

        IMedia ChunkUploadBinary(byte[] binary, string mediaType);
        IMedia UploadVideo(byte[] binary, string mediaType = "video/mp4");
    }

    public class UploadQueryExecutor : IUploadQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<IMedia> _mediaFactory;
        private readonly IFactory<IChunkedUploader> _chunkedUploadFactory;

        public UploadQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IFactory<IMedia> mediaFactory,
            IFactory<IChunkedUploader> chunkedUploadFactory)
        {
            _twitterAccessor = twitterAccessor;
            _mediaFactory = mediaFactory;
            _chunkedUploadFactory = chunkedUploadFactory;
        }

        public IEnumerable<IMedia> UploadBinaries(IEnumerable<byte[]> binaries)
        {
            var medias = new List<IMedia>();

            foreach (var binary in binaries)
            {
                var media = _mediaFactory.Create();
                media.Data = binary;
                medias.Add(media);
            }

            UploadMedias(medias, false);
            return medias;
        }

        public IMedia UploadBinary(byte[] binary)
        {
            var medias = UploadBinaries(new[] { binary });

            if (medias == null)
            {
                return null;
            }

            return medias.SingleOrDefault(x => x.UploadedMediaInfo != null);
        }

        public void UploadMedias(IEnumerable<IMedia> medias, bool forceReUpload = true)
        {
            if (medias == null)
            {
                return;
            }

            var mediaArray = medias.ToArray();

            if (forceReUpload)
            {
                mediaArray.ForEach(x => x.UploadedMediaInfo = null);
            }

            // Twitter documentation states that an image uploaded on Twitter is retained 60 minutes.
            // Tweetinvi is republishing a Media after 58 minutes after its first upload.
            var mediasToPublish = mediaArray.Where(x => !x.HasBeenUploaded || DateTime.Now.Subtract(x.UploadedMediaInfo.CreatedDate).TotalMinutes > 58);

            foreach (var mediaToPublish in mediasToPublish)
            {
                var binaries = new[] { mediaToPublish.Data };
                mediaToPublish.UploadedMediaInfo = _twitterAccessor.ExecuteMultipartQuery<IUploadedMediaInfo>(Resources.Upload_URL, binaries);
            }
        }

        public IMedia ChunkUploadBinary(byte[] binary, string mediaType)
        {
            var uploader = CreateChunkedUploader();

            if (uploader.Init(mediaType, binary.Length))
            {
                if (uploader.Append(binary))
                {
                    return uploader.Complete();
                }
            }

            return null;
        }

        public IMedia UploadVideo(byte[] binary, string mediaType = "video/mp4")
        {
            return ChunkUploadBinary(binary, mediaType);
        }

        public IChunkedUploader CreateChunkedUploader()
        {
            return _chunkedUploadFactory.Create();
        }
    }
}