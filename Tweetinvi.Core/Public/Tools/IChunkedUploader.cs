using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public interface IChunkedUploader
    {
        long? MediaId { get; set; }
        int NextSegmentIndex { get; set; }
        Dictionary<long, byte[]> UploadedSegments { get; }

        Task<bool> Init(string mediaType, int totalBinaryLength);
        Task<bool> Append(byte[] binary, string mediaType, TimeSpan? timeout = null, int? segmentIndex = null);
        Task<bool> Append(IChunkUploadAppendParameters parameters);
        Task<IMedia> Complete();
        Task<bool> Init(IChunkUploadInitParameters initParameters);
    }
}