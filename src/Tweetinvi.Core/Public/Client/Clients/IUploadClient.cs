using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IUploadClient
    {
        /// <summary>
        /// Validate all the Upload client parameters
        /// </summary>
        IUploadClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="IUploadClient.UploadBinaryAsync(IUploadParameters)" />
        Task<IMedia> UploadBinaryAsync(byte[] binary);

        /// <summary>
        /// Upload a binary in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadBinaryAsync(IUploadParameters parameters);

        /// <inheritdoc cref="IUploadClient.UploadTweetImageAsync(IUploadTweetImageParameters)" />
        Task<IMedia> UploadTweetImageAsync(byte[] binary);

        /// <summary>
        /// Upload an image to Twitter
        /// </summary>
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadTweetImageAsync(IUploadTweetImageParameters parameters);

        /// <inheritdoc cref="IUploadClient.UploadMessageImageAsync(IUploadMessageImageParameters)" />
        Task<IMedia> UploadMessageImageAsync(byte[] binary);

        /// <summary>
        /// Upload an image to Twitter
        /// </summary>
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadMessageImageAsync(IUploadMessageImageParameters parameters);

        /// <inheritdoc cref="IUploadClient.UploadTweetVideoAsync(IUploadTweetVideoParameters)" />
        Task<IMedia> UploadTweetVideoAsync(byte[] binary);

        /// <summary>
        /// Upload a video in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <para>STATUS : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadTweetVideoAsync(IUploadTweetVideoParameters parameters);

        /// <inheritdoc cref="IUploadClient.UploadMessageVideoAsync(IUploadMessageVideoParameters)" />
        Task<IMedia> UploadMessageVideoAsync(byte[] binary);

        /// <summary>
        /// Upload a video in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <para>STATUS : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Uploaded media</returns>
        Task<IMedia> UploadMessageVideoAsync(IUploadMessageVideoParameters parameters);

        /// <inheritdoc cref="IUploadClient.AddMediaMetadataAsync(IAddMediaMetadataParameters)" />
        Task AddMediaMetadataAsync(IMediaMetadata metadata);

        /// <summary>
        /// Add metadata to an uploaded media
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-metadata-create</para>
        /// </summary>
        Task AddMediaMetadataAsync(IAddMediaMetadataParameters parameters);

        /// <summary>
        /// Get a video processing status
        /// <para>https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Current status of the video processing</returns>
        Task<IUploadedMediaInfo> GetVideoProcessingStatusAsync(IMedia media);

        /// <summary>
        /// Wait for the upload of a media has completed
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Completes wait the media is ready for use</returns>
        Task WaitForMediaProcessingToGetAllMetadataAsync(IMedia media);
    }
}