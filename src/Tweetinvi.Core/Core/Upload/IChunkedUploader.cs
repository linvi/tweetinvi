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

        Task<bool> InitAsync(IChunkUploadInitParameters initParameters, ITwitterRequest request);
        Task<bool> AppendAsync(IChunkUploadAppendParameters parameters, ITwitterRequest request);
        Task<bool> FinalizeAsync(ICustomRequestParameters customRequestParameters, ITwitterRequest request);
    }
}