namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IEditableMedia : IMedia
    {
        new long? MediaId { get; set; }
    }

    public interface IMedia
    {
        string Name { get; set; }
        byte[] Data { get; set; }
        long? MediaId { get; }
        string ContentType { get; set; }

        bool HasBeenUploaded { get; }
        IUploadedMediaInfo UploadedMediaInfo { get; set; }

        IMedia CloneWithoutMediaInfo(IMedia source);
    }
}