using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUploadQueryGenerator
    {
        string GetChunkedUploadInitQuery(IChunkUploadInitParameters chunkUploadInitParameters);
        string GetChunkedUploadAppendQuery(IChunkUploadAppendParameters parameters);
        string GetChunkedUploadFinalizeQuery(long mediaId);
    }
}