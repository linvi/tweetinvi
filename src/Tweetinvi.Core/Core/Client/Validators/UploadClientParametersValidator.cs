using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IUploadClientParametersValidator
    {
        void Validate(IUploadParameters parameters);
        void Validate(IAddMediaMetadataParameters parameters);
    }

    public class UploadClientParametersValidator : IUploadClientParametersValidator
    {
        private readonly ITwitterClient _client;
        private readonly IUploadClientRequiredParametersValidator _uploadClientRequiredParametersValidator;

        public UploadClientParametersValidator(
            ITwitterClient client,
            IUploadClientRequiredParametersValidator uploadClientRequiredParametersValidator)
        {
            _client = client;
            _uploadClientRequiredParametersValidator = uploadClientRequiredParametersValidator;
        }

        public TwitterLimits Limits => _client.Config.Limits;

        public void Validate(IUploadParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);

            if (parameters.MediaCategory == MediaCategory.Gif || parameters.MediaCategory == MediaCategory.Image ||
                parameters.MediaCategory == MediaCategory.DmGif || parameters.MediaCategory == MediaCategory.DmImage)
            {
                var maxUploadSize = Limits.UPLOAD_MAX_IMAGE_SIZE;
                if (parameters.Binary.Length > maxUploadSize)
                {
                    throw new TwitterArgumentLimitException($"{nameof(parameters.Binary)}", maxUploadSize, nameof(Limits.UPLOAD_MAX_IMAGE_SIZE), "binary size");
                }
            }

            if (parameters.MediaCategory == MediaCategory.Video || parameters.MediaCategory == MediaCategory.DmVideo)
            {
                var maxUploadSize = Limits.UPLOAD_MAX_VIDEO_SIZE;
                if (parameters.Binary.Length > maxUploadSize)
                {
                    throw new TwitterArgumentLimitException($"{nameof(parameters.Binary)}", maxUploadSize, nameof(Limits.UPLOAD_MAX_VIDEO_SIZE), "binary size");
                }
            }
        }

        public void Validate(IAddMediaMetadataParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);
        }
    }
}