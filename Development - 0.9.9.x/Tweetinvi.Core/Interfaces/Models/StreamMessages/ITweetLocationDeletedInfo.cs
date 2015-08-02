namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface ITweetLocationDeletedInfo
    {
        long UserId { get; }
        string UserIdStr { get; }
        long UpToStatusId { get; }
        string UpToStatusIdStr { get; }
    }
}