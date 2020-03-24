namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more description visit : https://dev.twitter.com/rest/media/uploading-media
    /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
    /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
    /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
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

        public UploadParameters(byte[] binary)
        {
            Binary = binary;
        }

        public byte[] Binary { get; set; }
    }
}