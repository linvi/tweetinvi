namespace Tweetinvi.Models.DTO
{
    public interface IUploadedImageDetails
    {
        int? Width { get; set; }
        int? Height { get; set; }
        string ImageType { get; set; }
    }
}