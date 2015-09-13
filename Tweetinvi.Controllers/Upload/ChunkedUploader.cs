using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;

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
            var initQuery = _uploadQueryGenerator.GetChunkedUploadInitQuery(mediaType, totalBinaryLength);

            var initModel = _twitterAccessor.ExecutePOSTQuery<UploadInitModel>(initQuery);
            if (initModel != null)
            {
                _expectedBinaryLength = totalBinaryLength;
                _media.MediaId = initModel.MediaId;
            }

            return initModel != null;
        }

        public bool Append(byte[] binary, int? segmentIndex = null)
        {
            if (MediaId == null)
            {
                throw new InvalidOperationException("You cannot append content to a non initialized chunked upload. You need to invoke the initialize method OR set the MediaId property of an existing ChunkedUpload.");
            }

            if (segmentIndex == null)
            {
                segmentIndex = NextSegmentIndex;
            }

            var appendQuery = _uploadQueryGenerator.GetChunkedUploadAppendQuery(MediaId.Value, segmentIndex.Value);
            var sucess = _twitterAccessor.TryExecuteMultipartQuery(appendQuery, new[] { binary });

            if (sucess)
            {
                UploadedSegments.Add(segmentIndex.Value, binary);
                ++NextSegmentIndex;
            }

            return sucess;
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