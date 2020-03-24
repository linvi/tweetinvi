namespace Tweetinvi.Models.DTO
{
    public interface ISearchMetadata
    {
        double CompletedIn { get; }
        long MaxId { get; }
        string MaxIdStr { get; }
        string NextResults { get; }
        string Query { get; }
        string RefreshURL { get; }
        int Count { get; }
        long SinceId { get; }
        string SinceIdStr { get; }
    }
}
