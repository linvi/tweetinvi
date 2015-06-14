namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IMedia
    {
        string Name { get; set; }
        byte[] Data { get; set; }
        long? MediaId { get; }

        bool HasBeenUploaded { get; }
        IUploadedMediaInfo UploadedMediaInfo { get; set; }

        IMedia CloneWithoutMediaInfo(IMedia source);
    }
}