using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Upload;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadQueryExecutor
    {
        /// <summary>
        /// Upload a binary
        /// </summary>
        Task<IChunkUploadResult> UploadBinary(IUploadParameters uploadQueryParameters, ITwitterRequest request);

        /// <summary>
        /// Add metadata to a media that has been uploaded.
        /// </summary>
        Task<ITwitterResult> AddMediaMetadata(IAddMediaMetadataParameters metadata, ITwitterRequest request);
    }

    public class UploadQueryExecutor : IUploadQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<IChunkedUploader> _chunkedUploadFactory;
        private readonly IUploadHelper _uploadHelper;

        public UploadQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IFactory<IChunkedUploader> chunkedUploadFactory,
            IUploadHelper uploadHelper)
        {
            _twitterAccessor = twitterAccessor;
            _chunkedUploadFactory = chunkedUploadFactory;
            _uploadHelper = uploadHelper;
        }

        public async Task<IChunkUploadResult> UploadBinary(IUploadParameters uploadQueryParameters, ITwitterRequest baseRequest)
        {
            var binary = uploadQueryParameters.Binary;
            var uploader = CreateChunkedUploader();

            var initParameters = new ChunkUploadInitParameters
            {
                TotalBinaryLength = binary.Length,
                MediaType = uploadQueryParameters.QueryMediaType,
                MediaCategory = uploadQueryParameters.QueryMediaCategory,
                AdditionalOwnerIds = uploadQueryParameters.AdditionalOwnerIds,
                CustomRequestParameters = uploadQueryParameters.InitCustomRequestParameters,
            };

            var initRequest = new TwitterRequest(baseRequest);
            var initOperationSucceeded = await uploader.Init(initParameters, initRequest).ConfigureAwait(false);

            if (!initOperationSucceeded)
            {
                return uploader.Result;
            }
            
            var binaryChunks = GetBinaryChunks(binary, uploadQueryParameters.MaxChunkSize);

            var totalSize = await CalculateSize("media", binary).ConfigureAwait(false);
            long uploadedSize = 0;

            uploadQueryParameters.UploadStateChanged?.Invoke(new MediaUploadProgressChangedEventArgs(UploadProgressState.INITIALIZED, 0, totalSize));

            for (var i = 0; i < binaryChunks.Count; ++i)
            {
                var binaryChunk = binaryChunks[i];
                var startUploadedSize = uploadedSize;

                var appendParameters = new ChunkUploadAppendParameters(
                    binaryChunk,
                    "media", // Must be `media`, if using the real media type as content id, Twitter does not accept when invoking .Finalize().
                    uploadQueryParameters.Timeout)
                {
                    UploadProgressChanged = (args) =>
                    {
                        uploadedSize = startUploadedSize + args.NumberOfBytesUploaded;
                        uploadQueryParameters.UploadStateChanged?.Invoke(new MediaUploadProgressChangedEventArgs(UploadProgressState.PROGRESS_CHANGED, uploadedSize, totalSize));
                    },
                    CustomRequestParameters = uploadQueryParameters.AppendCustomRequestParameters
                };

                var appendRequest = new TwitterRequest(baseRequest);
                var appendOperationSucceeded = await uploader.Append(appendParameters, appendRequest).ConfigureAwait(false);

                if (!appendOperationSucceeded)
                {
                    uploadQueryParameters.UploadStateChanged?.Invoke(new MediaUploadProgressChangedEventArgs(UploadProgressState.FAILED, uploadedSize, totalSize));
                    return uploader.Result;
                }
            }

            var finalizeRequest = new TwitterRequest(baseRequest);

            var finalizeSucceeded = await uploader.Finalize(uploadQueryParameters.FinalizeCustomRequestParameters, finalizeRequest).ConfigureAwait(false);
            if (finalizeSucceeded)
            {
                var result = uploader.Result;
                uploadQueryParameters.UploadStateChanged?.Invoke(new MediaUploadProgressChangedEventArgs(UploadProgressState.COMPLETED, uploadedSize, totalSize));

                var category = uploadQueryParameters.MediaCategory;
                var isAwaitableUpload = category == MediaCategory.Gif || category == MediaCategory.Video;
                
                if (isAwaitableUpload && uploadQueryParameters.WaitForTwitterProcessing)
                {
                    var request = new TwitterRequest(baseRequest);
                    await _uploadHelper.WaitForMediaProcessingToGetAllMetadata(result.Media, request).ConfigureAwait(false);
                }
            }

            return uploader.Result;
        }

        private static async Task<long> CalculateSize(string contentId, byte[] binary)
        {
            using (var httpContent = MultipartTwitterQuery.CreateHttpContent(contentId, new[] { binary }))
            {
                return (await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false)).Length;
            }
        }

        private static List<byte[]> GetBinaryChunks(byte[] binary, int chunkSize)
        {
            var result = new List<byte[]>();
            var numberOfChunks = (int) Math.Ceiling((double) binary.Length / chunkSize);

            for (var i = 0; i < numberOfChunks; ++i)
            {
                var skip = i * chunkSize;
                var take = Math.Min(chunkSize, binary.Length - skip);

                var chunkBytes = binary.Skip(skip).Take(take).ToArray();

                result.Add(chunkBytes);
            }

            return result;
        }

        private IChunkedUploader CreateChunkedUploader()
        {
            return _chunkedUploadFactory.Create();
        }

        public Task<ITwitterResult> AddMediaMetadata(IAddMediaMetadataParameters metadata, ITwitterRequest request)
        {
            var json = JsonConvert.SerializeObject(metadata);

            request.Query.Url = "https://upload.twitter.com/1.1/media/metadata/create.json";
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.HttpContent = new StringContent(json);

            return _twitterAccessor.ExecuteRequest(request);
        }
        
        public async Task<ITwitterResult<IUploadedMediaInfo>> GetMediaStatus(IMedia media, bool autoWait, ITwitterRequest request)
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
                    await Task.Delay((int) waitTimeRemaining.TotalMilliseconds).ConfigureAwait(false);
                }
            }

            request.Query.Url = $"https://upload.twitter.com/1.1/media/upload.json?command=STATUS&media_id={media.Id}";
            request.Query.HttpMethod = HttpMethod.GET;
            
            return await _twitterAccessor.ExecuteRequest<IUploadedMediaInfo>(request).ConfigureAwait(false);
        }
    }
}