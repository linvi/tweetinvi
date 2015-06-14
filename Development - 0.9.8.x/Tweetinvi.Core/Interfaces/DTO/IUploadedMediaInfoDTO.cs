namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IUploadedMediaInfoDTO
    {
        int? Width { get; set; }
        int? Height { get; set; }
        string ImageType { get; set; }
    }
}