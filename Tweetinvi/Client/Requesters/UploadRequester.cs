using System.Threading.Tasks;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Upload;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class UploadRequester : BaseRequester, IUploadRequester
    {
        private readonly IUploadClientRequiredParametersValidator _uploadClientRequiredParametersValidator;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly IUploadMediaStatusQueryExecutor _uploadMediaStatusQueryExecutor;
        private readonly IUploadHelper _uploadHelper;

        public UploadRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IUploadClientRequiredParametersValidator uploadClientRequiredParametersValidator,
            IUploadQueryExecutor uploadQueryExecutor,
            IUploadMediaStatusQueryExecutor uploadMediaStatusQueryExecutor,
            IUploadHelper uploadHelper)
        : base(client, clientEvents)
        {
            _uploadClientRequiredParametersValidator = uploadClientRequiredParametersValidator;
            _uploadQueryExecutor = uploadQueryExecutor;
            _uploadMediaStatusQueryExecutor = uploadMediaStatusQueryExecutor;
            _uploadHelper = uploadHelper;
        }

        public Task<IChunkUploadResult> UploadBinary(IUploadParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _uploadQueryExecutor.UploadBinary(parameters, request));
        }

        public Task<ITwitterResult> AddMediaMetadata(IAddMediaMetadataParameters parameters)
        {
            _uploadClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _uploadQueryExecutor.AddMediaMetadata(parameters, request));
        }

        public Task<ITwitterResult<IUploadedMediaInfo>> GetVideoProcessingStatus(IMedia media)
        {
            return ExecuteRequest(request => _uploadMediaStatusQueryExecutor.GetMediaStatus(media, request));
        }

        public Task WaitForMediaProcessingToGetAllMetadata(IMedia media)
        {
            return ExecuteRequest(request => _uploadHelper.WaitForMediaProcessingToGetAllMetadata(media, request));
        }
    }
}