namespace Tweetinvi.Core.Public.Parameters
{
    public interface IUploadVideoOptionalParameters : IUploadOptionalParameters
    {

    }

    public class UploadVideoOptionalParameters : UploadOptionalParameters, IUploadVideoOptionalParameters
    {
        public UploadVideoOptionalParameters()
        {
            MediaType = Models.Enum.MediaType.VideoMp4;
            MediaCategory = Models.Enum.MediaCategory.Video;
        }
    }
}
