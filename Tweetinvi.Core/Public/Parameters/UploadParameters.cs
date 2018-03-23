using Tweetinvi.Core.Public.Parameters;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/media/uploading-media
    /// </summary>
    public interface IUploadParameters : IUploadOptionalParameters
    {
        /// <summary>
        /// Binary that you want to publish
        /// </summary>
        byte[] Binary { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/media/uploading-media
    /// </summary>
    public class UploadParameters : UploadOptionalParameters, IUploadParameters
    {

        public UploadParameters(byte[] binary = null)
        {
            Binary = binary;
        }

        public byte[] Binary { get; set; }
    }
}