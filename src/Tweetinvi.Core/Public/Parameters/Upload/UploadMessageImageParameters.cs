namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more description visit : https://dev.twitter.com/rest/media/uploading-media
    /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
    /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
    /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
    /// </summary>
    public interface IUploadMessageImageParameters : IUploadParameters
    {
    }

    public class UploadMessageImageParameters : UploadBinaryParameters, IUploadMessageImageParameters
    {
        public UploadMessageImageParameters(byte[] binary) : base(binary)
        {
            MediaType = Models.MediaType.Media;
            MediaCategory = Models.MediaCategory.DmImage;
        }
    }
}