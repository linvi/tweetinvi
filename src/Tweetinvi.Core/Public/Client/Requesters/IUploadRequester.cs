using System.Threading.Tasks;
using Tweetinvi.Core.Upload;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IUploadRequester
    {
        /// <summary>
        /// Upload a binary in chunks and waits for the Twitter to have processed it
        /// <para>INIT : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-init</para>
        /// <para>APPEND : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-append</para>
        /// <para>FINALIZE : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-upload-finalize</para>
        /// </summary>
        /// <returns>Uploaded media information</returns>
        Task<IChunkUploadResult> UploadBinaryAsync(IUploadParameters parameters);
        
        /// <summary>
        /// Add metadata to an uploaded media
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/post-media-metadata-create</para>
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        Task<ITwitterResult> AddMediaMetadataAsync(IAddMediaMetadataParameters parameters);
        
        /// <summary>
        /// Get a video processing status
        /// <para>https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Current status of the video processing</returns>
        Task<ITwitterResult<IUploadedMediaInfo>> GetVideoProcessingStatusAsync(IMedia media);
        
        /// <summary>
        /// Wait for the upload of a media has completed
        /// <para>Read more : https://dev.twitter.com/en/docs/media/upload-media/api-reference/get-media-upload-status</para>
        /// </summary>
        /// <returns>Completes wait the media is ready for use</returns>
        Task WaitForMediaProcessingToGetAllMetadataAsync(IMedia media);
    }
}