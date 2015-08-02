using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Controllers.Transactions
{
    public interface IChunkedUploader
    {
        long? MediaId { get; set; }
        int NextSegmentIndex { get; set; }
        Dictionary<long, byte[]> UploadedSegments { get; }

        bool Init(string mediaType, int totalBinaryLength);
        bool Append(byte[] binary, int? segmentIndex = null);
        IMedia Complete();
    }
}