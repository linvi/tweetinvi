using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Interfaces.Controllers.Transactions
{
    public interface IChunkedUploader
    {
        long? MediaId { get; set; }
        int NextSegmentIndex { get; set; }
        Dictionary<long, byte[]> UploadedSegments { get; }

        bool Init(string mediaType, int totalBinaryLength);
        bool Append(byte[] binary, string mediaType, TimeSpan? timeout = null, int ? segmentIndex = null);
        bool Append(IChunkUploadAppendParameters parameters);
        IMedia Complete();
        bool Init(IChunkUploadInitParameters initParameters);
    }
}