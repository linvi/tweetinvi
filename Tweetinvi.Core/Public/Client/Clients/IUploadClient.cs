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
        /// <returns>Uploaded media information</returns>
        Task<IMedia> UploadBinary(IUploadParameters parameters);

        /// <inheritdoc cref="UploadVideo(IUploadVideoParameters)" />
        Task<IMedia> UploadVideo(byte[] binary);

        /// <summary>
        /// Upload a video in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// <para>STATUS : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Uploaded media information</returns>
        Task<IMedia> UploadVideo(IUploadVideoParameters parameters);

        /// <inheritdoc cref="AddMediaMetadata(IAddMediaMetadataParameters)" />
        Task<bool> AddMediaMetadata(IMediaMetadata metadata);
        
        /// <summary>
        /// Add metadata to an uploaded media
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-metadata-create</para>
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        Task<bool> AddMediaMetadata(IAddMediaMetadataParameters parameters);

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