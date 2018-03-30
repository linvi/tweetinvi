using Tweetinvi.Core.Public.Models.Enum;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Public.Parameters
{
    public interface IUploadVideoParameters : IUploadParameters
    {
    }

    public class UploadVideoParameters : UploadParameters, IUploadVideoParameters
    {
        public UploadVideoParameters(byte[] binary = null) : base(binary)
        {
            MediaType = Models.Enum.MediaType.VideoMp4;
            MediaCategory = Models.Enum.MediaCategory.Video;
        }
    }
}
