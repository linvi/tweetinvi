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

        public UploadClient(ITwitterClient client)
        {
            _uploadRequester = client.Raw.Upload;
        }

        public Task<IMedia> UploadBinary(byte[] binary)
        {
            return UploadBinary(new UploadBinaryParameters(binary));
        }

        public async Task<IMedia> UploadBinary(IUploadParameters parameters)
        {
            var chunkUploadResult = await _uploadRequester.UploadBinary(parameters).ConfigureAwait(false);
            return chunkUploadResult.Media;
        }

        public Task<IMedia> UploadTweetImage(byte[] binary)
        {
            return UploadTweetImage(new UploadTweetImageParameters(binary));
        }

        public Task<IMedia> UploadTweetImage(IUploadTweetImageParameters parameters)
        {
            return UploadBinary(parameters);
        }

        public Task<IMedia> UploadMessageImage(byte[] binary)
        {
            return UploadMessageImage(new UploadMessageImageParameters(binary));
        }

        public Task<IMedia> UploadMessageImage(IUploadMessageImageParameters parameters)
        {
            return UploadBinary(parameters);
        }

        public Task<IMedia> UploadTweetVideo(byte[] binary)
        {
            return UploadTweetVideo(new UploadTweetVideoParameters(binary));
        }

        public Task<IMedia> UploadTweetVideo(IUploadTweetVideoParameters parameters)
        {
            return UploadBinary(parameters);
        }

        public Task<IMedia> UploadMessageVideo(byte[] binary)
        {
            return UploadMessageVideo(new UploadMessageVideoParameters(binary));
        }

        public Task<IMedia> UploadMessageVideo(IUploadMessageVideoParameters parameters)
        {
            return UploadBinary(parameters);
        }

        public Task AddMediaMetadata(IMediaMetadata metadata)
        {
            return AddMediaMetadata(new AddMediaMetadataParameters(metadata.MediaId)
            {
                AltText = metadata.AltText
            });
        }

        public async Task AddMediaMetadata(IAddMediaMetadataParameters parameters)
        {
            await _uploadRequester.AddMediaMetadata(parameters).ConfigureAwait(false);
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