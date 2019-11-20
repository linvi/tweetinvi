using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IUploadClientParametersValidator
    {
        void Validate(IUploadParameters parameters);
        void Validate(IAddMediaMetadataParameters parameters);
    }
    
    public interface IInternalUploadClientParametersValidator : IUploadClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class UploadClientParametersValidator : IInternalUploadClientParametersValidator
    {
        private readonly IUploadClientRequiredParametersValidator _uploadClientRequiredParametersValidator;

        public UploadClientParametersValidator(IUploadClientRequiredParametersValidator uploadClientRequiredParametersValidator)
        {
            _uploadClientRequiredParametersValidator = uploadClientRequiredParametersValidator;
        }

        public void Initialize(ITwitterClient client)
        {
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