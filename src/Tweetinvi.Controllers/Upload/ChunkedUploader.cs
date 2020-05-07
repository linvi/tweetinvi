using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Upload;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Controllers.Upload
{
    public class ChunkedUploader : IChunkedUploader
    {
        private readonly IMedia _media;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUploadQueryGenerator _uploadQueryGenerator;

        private int? _expectedBinaryLength;
        private readonly ChunkUploadResult _result;

        public ChunkedUploader(
            ITwitterAccessor twitterAccessor,
            IUploadQueryGenerator uploadQueryGenerator,
            IMedia media)
        {
            _twitterAccessor = twitterAccessor;
            _uploadQueryGenerator = uploadQueryGenerator;
            _media = media;
            _result = new ChunkUploadResult
            {
                Media = _media
            };

            UploadedSegments = new Dictionary<long, byte[]>();
        }

        public long? MediaId
        {
            get => _media.Id;
            set => _media.Id = value;
        }

        public Dictionary<long, byte[]> UploadedSegments { get; }
        public int NextSegmentIndex { get; set; }

        public async Task<bool> InitAsync(IChunkUploadInitParameters initParameters, ITwitterRequest request)
        {
            var initQuery = _uploadQueryGenerator.GetChunkedUploadInitQuery(initParameters);

            request.Query.Url = initQuery;
            request.Query.HttpMethod = HttpMethod.POST;

            var twitterResult = await _twitterAccessor.ExecuteRequestAsync<IUploadInitModel>(request).ConfigureAwait(false);
            _result.Init = twitterResult;

            var initModel = twitterResult?.DataTransferObject;
            if (initModel != null)
            {
                _expectedBinaryLength = initParameters.TotalBinaryLength;
                _media.Id = initModel.MediaId;
            }

            return initModel != null;
        }

        public async Task<bool> AppendAsync(IChunkUploadAppendParameters parameters, ITwitterRequest request)
        {
            if (MediaId == null)
            {
                throw new InvalidOperationException("You cannot append content to a non initialized chunked upload. You need to invoke the initialize method OR set the MediaId property of an existing ChunkedUpload.");
            }

            if (parameters.SegmentIndex == null)
            {
                parameters.SegmentIndex = NextSegmentIndex;
            }

            if (parameters.MediaId == null)
            {
                parameters.MediaId = MediaId;
            }

            var appendQuery = _uploadQueryGenerator.GetChunkedUploadAppendQuery(parameters);

            var multipartQuery = new MultipartTwitterQuery(request.Query)
            {
                Url = appendQuery,
                HttpMethod = HttpMethod.POST,
                Binaries = new[] { parameters.Binary },
                Timeout = parameters.Timeout ?? TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite),
                ContentId = parameters.MediaType,
                UploadProgressChanged = args =>
                {
                    var progressChangedEventArgs = new MediaUploadProgressChangedEventArgs(UploadProgressState.PROGRESS_CHANGED, args.NumberOfBytesUploaded, args.TotalOfBytesToUpload);
                    parameters.UploadProgressChanged(progressChangedEventArgs);
                },
            };

            request.Query = multipartQuery;

            var twitterResult = await _twitterAccessor.ExecuteRequestAsync(request).ConfigureAwait(false);
            _result.AppendsList.Add(twitterResult);

            if (twitterResult.Response.IsSuccessStatusCode)
            {
                UploadedSegments.Add(parameters.SegmentIndex.Value, parameters.Binary);
                ++NextSegmentIndex;
            }

            return twitterResult.Response.IsSuccessStatusCode;
        }

        public async Task<bool> FinalizeAsync(ICustomRequestParameters customRequestParameters, ITwitterRequest request)
        {
            if (MediaId == null)
            {
                throw new InvalidOperationException("You cannot complete a non initialized chunked upload. Please initialize the method, append some content and then complete the upload.");
            }

            var finalizeQuery = _uploadQueryGenerator.GetChunkedUploadFinalizeQuery(MediaId.Value, customRequestParameters);

            request.Query.Url = finalizeQuery;
            request.Query.HttpMethod = HttpMethod.POST;

            var finalizeTwitterResult = await _twitterAccessor.ExecuteRequestAsync<UploadedMediaInfo>(request).ConfigureAwait(false);
            var uploadedMediaInfos = finalizeTwitterResult.DataTransferObject;

            UpdateMedia(uploadedMediaInfos);

            _result.Finalize = finalizeTwitterResult;

            return finalizeTwitterResult.Response.IsSuccessStatusCode;
        }

        public IChunkUploadResult Result => _result;

        private void UpdateMedia(IUploadedMediaInfo uploadedMediaInfos)
        {
            _media.UploadedMediaInfo = uploadedMediaInfos;

            if (_expectedBinaryLength != null)
            {
                // If all the data has not been sent then we do not construct the data
                if (UploadedSegments.Sum(x => x.Value.Length) == _expectedBinaryLength)
                {
                    var allSegments = UploadedSegments.OrderBy(x => x.Key);
                    _media.Data = allSegments.SelectMany(x => x.Value).ToArray();
                }
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class UploadInitModel
        {
            [JsonProperty("media_id")] public long MediaId { get; set; }

            [JsonProperty("expires_after_secs")] public long ExpiresAfterInSeconds { get; set; }
        }
    }
}