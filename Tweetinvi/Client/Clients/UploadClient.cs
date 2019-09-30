using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class UploadClient : IUploadClient
    {
        private readonly IUploadRequester _uploadRequester;

        public UploadClient(TwitterClient client)
        {
            _uploadRequester = client.RequestExecutor.Upload;
        }
        
        public async Task<IMedia> UploadBinary(IUploadParameters parameters)
        {
            var chunkUploadResult = await _uploadRequester.UploadBinary(parameters).ConfigureAwait(false);
            return chunkUploadResult.Media;
        }

        public Task<IMedia> UploadVideo(IUploadVideoParameters parameters)
        {
            return UploadBinary(parameters);
        }

        public Task<bool> AddMediaMetadata(IMediaMetadata metadata)
        {
            return AddMediaMetadata(new AddMediaMetadataParameters(metadata.MediaId)
            {
                AltText = metadata.AltText
            });
        }
        
        public async Task<bool> AddMediaMetadata(IAddMediaMetadataParameters parameters)
        {
            var twitterResult = await _uploadRequester.AddMediaMetadata(parameters).ConfigureAwait(false);
            return twitterResult.Response.IsSuccessStatusCode;
        }

        public async Task<IUploadedMediaInfo> GetVideoProcessingStatus(IMedia media)
        {
            var twitterResult = await _uploadRequester.GetVideoProcessingStatus(media).ConfigureAwait(false);
            return twitterResult.DataTransferObject;
        }

        public Task WaitForMediaProcessingToGetAllMetadata(IMedia media)
        {
            return _uploadRequester.WaitForMediaProcessingToGetAllMetadata(media);
        }
    }
}