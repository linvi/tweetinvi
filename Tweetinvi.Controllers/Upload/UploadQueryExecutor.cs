using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Core.Web;

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

        /// <summary>
        /// Upload a binary in multiple queries.
        /// </summary>
        IMedia ChunkUploadBinary(byte[] binary, string mediaType);

        /// <summary>
        /// Upload a binary in multiple queries.
        /// </summary>
        IMedia ChunkUploadBinary(IUploadQueryParameters uploadQueryParameters);

        /// <summary>
        /// Upload a video in multiple queries if necessary.
        /// </summary>
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
                if (mediaToPublish.Data.Length < TweetinviConsts.UPLOAD_MAX_CHUNK_SIZE)
                {
                    var multipartHttpRequestParameters = new MultipartHttpRequestParameters
                    {
                        Query = Resources.Upload_URL,
                        Binaries = new List<byte[]> { mediaToPublish.Data }
                    };

                    mediaToPublish.UploadedMediaInfo = _twitterAccessor.ExecuteMultipartQuery<IUploadedMediaInfo>(multipartHttpRequestParameters);
                }
                else
                {
                    var uploadQueryParameters = new UploadQueryParameters
                    {
                        Binaries = new List<byte[]> {mediaToPublish.Data}
                    };

                    var media = ChunkUploadBinary(uploadQueryParameters);
                    mediaToPublish.UploadedMediaInfo = media.UploadedMediaInfo;
                }
            }
        }

        public IMedia ChunkUploadBinary(byte[] binary, string mediaType)
        {
            var parameters = new UploadQueryParameters()
            {
                Binaries = new List<byte[]> { binary },
                MediaType = mediaType,
            };

            return ChunkUploadBinary(parameters);
        }

        public IMedia ChunkUploadBinary(IUploadQueryParameters uploadQueryParameters)
        {
            if (uploadQueryParameters.Binaries.Count != 1)
            {
                throw new ArgumentException("ChunkUpload binary can only upload 1 binary at a time.");
            }

            var binary = uploadQueryParameters.Binaries[0];
            var uploader = CreateChunkedUploader();

            var initParameters = new ChunkUploadInitParameters
            {
                TotalBinaryLength = binary.Length,
                MediaType = uploadQueryParameters.MediaType,
                AdditionalOwnerIds = uploadQueryParameters.AdditionalOwnerIds,
                CustomRequestParameters = uploadQueryParameters.InitCustomRequestParameters,
            };

            if (uploader.Init(initParameters))
            {
                var binaryChunks = GetBinaryChunks(binary, uploadQueryParameters.MaxChunkSize);

                var totalsize = 0;

                foreach (var binaryChunk in binaryChunks)
                {
                    totalsize += binaryChunk.Length;
                    var appendParameters = new ChunkUploadAppendParameters(
                        binaryChunk, 
                        "media", // Must be media, if using the real media type as content id, Twitter does not accept when invoking .Finalize().
                        uploadQueryParameters.Timeout);

                    appendParameters.CustomRequestParameters = uploadQueryParameters.AppendCustomRequestParameters;

                    if (!uploader.Append(appendParameters))
                    {
                        return null;
                    }
                }

                var isTrue = totalsize == binary.Length;

                return uploader.Complete();
            }

            return null;
        }

        private List<byte[]> GetBinaryChunks(byte[] binary, int chunkSize)
        {
            var result = new List<byte[]>();
            var numberOfChunks = (int)Math.Ceiling((double)binary.Length / chunkSize);

            for (int i = 0; i < numberOfChunks; ++i)
            {
                var skip = i * chunkSize;
                var take = Math.Min(chunkSize, binary.Length - skip);

                var elts = binary.Skip(skip).Take(take).ToArray();

                result.Add(elts);
            }

            return result;
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