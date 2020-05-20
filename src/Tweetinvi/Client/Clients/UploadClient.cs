using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class UploadClient : IUploadClient
    {
        private readonly ITwitterClient _client;
        private readonly IUploadRequester _uploadRequester;

        public UploadClient(ITwitterClient client)
        {
            _client = client;
            _uploadRequester = client.Raw.Upload;
        }

        public IUploadClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<IMedia> UploadBinaryAsync(byte[] binary)
        {
            return UploadBinaryAsync(new UploadBinaryParameters(binary));
        }

        public async Task<IMedia> UploadBinaryAsync(IUploadParameters parameters)
        {
            var chunkUploadResult = await _uploadRequester.UploadBinaryAsync(parameters).ConfigureAwait(false);
            return chunkUploadResult.Media;
        }

        public Task<IMedia> UploadTweetImageAsync(byte[] binary)
        {
            return UploadTweetImageAsync(new UploadTweetImageParameters(binary));
        }

        public Task<IMedia> UploadTweetImageAsync(IUploadTweetImageParameters parameters)
        {
            return UploadBinaryAsync(parameters);
        }

        public Task<IMedia> UploadMessageImageAsync(byte[] binary)
        {
            return UploadMessageImageAsync(new UploadMessageImageParameters(binary));
        }

        public Task<IMedia> UploadMessageImageAsync(IUploadMessageImageParameters parameters)
        {
            return UploadBinaryAsync(parameters);
        }

        public Task<IMedia> UploadTweetVideoAsync(byte[] binary)
        {
            return UploadTweetVideoAsync(new UploadTweetVideoParameters(binary));
        }

        public Task<IMedia> UploadTweetVideoAsync(IUploadTweetVideoParameters parameters)
        {
            return UploadBinaryAsync(parameters);
        }

        public Task<IMedia> UploadMessageVideoAsync(byte[] binary)
        {
            return UploadMessageVideoAsync(new UploadMessageVideoParameters(binary));
        }

        public Task<IMedia> UploadMessageVideoAsync(IUploadMessageVideoParameters parameters)
        {
            return UploadBinaryAsync(parameters);
        }

        public Task AddMediaMetadataAsync(IMediaMetadata metadata)
        {
            return AddMediaMetadataAsync(new AddMediaMetadataParameters(metadata.MediaId)
            {
                AltText = metadata.AltText
            });
        }

        public async Task AddMediaMetadataAsync(IAddMediaMetadataParameters parameters)
        {
            await _uploadRequester.AddMediaMetadataAsync(parameters).ConfigureAwait(false);
        }

        public async Task<IUploadedMediaInfo> GetVideoProcessingStatusAsync(IMedia media)
        {
            var twitterResult = await _uploadRequester.GetVideoProcessingStatusAsync(media).ConfigureAwait(false);
            return twitterResult.Model;
        }

        public Task WaitForMediaProcessingToGetAllMetadataAsync(IMedia media)
        {
            return _uploadRequester.WaitForMediaProcessingToGetAllMetadataAsync(media);
        }
    }
}