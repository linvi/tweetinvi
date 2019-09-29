using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Upload
{
    public interface IChunkedUploader
    {
        long? MediaId { get; set; }
        int NextSegmentIndex { get; set; }
        Dictionary<long, byte[]> UploadedSegments { get; }
        IChunkUploadResult Result { get; }

        Task<bool> Init(IChunkUploadInitParameters initParameters, ITwitterRequest request);
        Task<bool> Append(IChunkUploadAppendParameters parameters, ITwitterRequest request);
        Task<bool> Finalize(ICustomRequestParameters customRequestParameters, ITwitterRequest request);
    }
}