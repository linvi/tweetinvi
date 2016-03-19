using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Upload
{
    public class ChunkedUploader : IChunkedUploader
    {
        private readonly IEditableMedia _media;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUploadQueryGenerator _uploadQueryGenerator;

        private int? _expectedBinaryLength;

        public ChunkedUploader(
            ITwitterAccessor twitterAccessor,
            IUploadQueryGenerator uploadQueryGenerator,
            IEditableMedia media)
        {
            _twitterAccessor = twitterAccessor;
            _uploadQueryGenerator = uploadQueryGenerator;
            _media = media;

            UploadedSegments = new Dictionary<long, byte[]>();
        }

        public long? MediaId
        {
            get { return _media.MediaId; }
            set { _media.MediaId = value; }
        }

        public Dictionary<long, byte[]> UploadedSegments { get; private set; }
        public int NextSegmentIndex { get; set; }

        public bool Init(string mediaType, int totalBinaryLength)
        {
            var parameters = new ChunkUploadInitParameters
            {
                MediaType = mediaType,
                TotalBinaryLength = totalBinaryLength
            };

            return Init(parameters);
        }

        public bool Init(IChunkUploadInitParameters initParameters)
        {
            var initQuery = _uploadQueryGenerator.GetChunkedUploadInitQuery(initParameters);

            var initModel = _twitterAccessor.ExecutePOSTQuery<UploadInitModel>(initQuery);
            if (initModel != null)
            {
                _expectedBinaryLength = initParameters.TotalBinaryLength;
                _media.MediaId = initModel.MediaId;
            }

            return initModel != null;
        }

        public bool Append(byte[] binary, string mediaType, TimeSpan? timeout = null, int? segmentIndex = null)
        {
            var parameters = new ChunkUploadAppendParameters(binary, mediaType, timeout);
            parameters.SegmentIndex = segmentIndex;
            return Append(parameters);
        }

        public bool Append(IChunkUploadAppendParameters parameters)
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

            var multiPartRequestParameters = new MultipartHttpRequestParameters
            {
                Query = appendQuery,
                Binaries = new List<byte[]> { parameters.Binary },
                Timeout = parameters.Timeout,
                ContentId = parameters.MediaType
            };

            var success = _twitterAccessor.TryExecuteMultipartQuery(multiPartRequestParameters);

            if (success)
            {
                UploadedSegments.Add(parameters.SegmentIndex.Value, parameters.Binary);
                ++NextSegmentIndex;
            }

            return success;
        }

        public IMedia Complete()
        {
            if (MediaId == null)
            {
                throw new InvalidOperationException("You cannot complete a non initialized chunked upload. Please initialize the method, append some content and then complete the upload.");
            }

            var finalizeQuery = _uploadQueryGenerator.GetChunkedUploadFinalizeQuery(MediaId.Value);
            var uploadedMediaInfos = _twitterAccessor.ExecutePOSTQuery<IUploadedMediaInfo>(finalizeQuery);

            UpdateMedia(uploadedMediaInfos);

            return _media;
        }

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
            [JsonProperty("media_id")]
            public long MediaId { get; set; }

            [JsonProperty("expires_after_secs")]
            public long ExpiresAfterInSeconds { get; set; }
        }
    }
}