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
        private ITwitterClient _client;

        public UploadClientParametersValidator(IUploadClientRequiredParametersValidator uploadClientRequiredParametersValidator)
        {
            _uploadClientRequiredParametersValidator = uploadClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.Config.Limits;

        public void Initialize(ITwitterClient client)
        {
            _client = client;
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