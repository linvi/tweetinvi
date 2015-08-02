namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IUploadQueryGenerator
    {
        string GetChunkedUploadInitQuery(string mediaType, long totalBinaryLength);
        string GetChunkedUploadAppendQuery(long mediaId, int segmentIndex);
        string GetChunkedUploadFinalizeQuery(long mediaId);
    }
}