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
        private readonly IUploadClientRequiredParametersValidator _uploadClientRequiredParametersValidator;

        public UploadClientParametersValidator(IUploadClientRequiredParametersValidator uploadClientRequiredParametersValidator)
        {
            _uploadClientRequiredParametersValidator = uploadClientRequiredParametersValidator;
        }

        public void Validate(IUploadParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IAddMediaMetadataParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);
        }
    }
}