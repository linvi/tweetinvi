using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IUploadClient
    {
        /// <inheritdoc cref="UploadBinary(IUploadParameters)" />
        Task<IMedia> UploadBinary(byte[] binary);

        /// <summary>
        /// Upload a binary in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadBinary(IUploadParameters parameters);

        /// <inheritdoc cref="UploadTweetImage(IUploadTweetImageParameters)" />
        Task<IMedia> UploadTweetImage(byte[] binary);

        /// <summary>
        /// Upload an image to Twitter
        /// </summary>
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadTweetImage(IUploadTweetImageParameters parameters);

        /// <inheritdoc cref="UploadMessageImage(IUploadMessageImageParameters)" />
        Task<IMedia> UploadMessageImage(byte[] binary);

        /// <summary>
        /// Upload an image to Twitter
        /// </summary>
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadMessageImage(IUploadMessageImageParameters parameters);

        /// <inheritdoc cref="UploadTweetVideo(IUploadTweetVideoParameters)" />
        Task<IMedia> UploadTweetVideo(byte[] binary);

        /// <summary>
        /// Upload a video in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <para>STATUS : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadTweetVideo(IUploadTweetVideoParameters parameters);

        /// <inheritdoc cref="UploadMessageVideo(IUploadMessageVideoParameters)" />
        Task<IMedia> UploadMessageVideo(byte[] binary);

        /// <summary>
        /// Upload a video in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <para>STATUS : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadMessageVideo(IUploadMessageVideoParameters parameters);

        /// <inheritdoc cref="AddMediaMetadata(IAddMediaMetadataParameters)" />
        Task AddMediaMetadata(IMediaMetadata metadata);

        /// <summary>
        /// Add metadata to an uploaded media
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-metadata-create</para>
        /// </summary>
        Task AddMediaMetadata(IAddMediaMetadataParameters parameters);

        /// <summary>
        /// Get a video processing status
        /// <para>https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Current status of the video processing</returns>
        Task<IUploadedMediaInfo> GetVideoProcessingStatus(IMedia media);

        /// <summary>
        /// Wait for the upload of a media has completed
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Completes wait the media is ready for use</returns>
        Task WaitForMediaProcessingToGetAllMetadata(IMedia media);
    }
}