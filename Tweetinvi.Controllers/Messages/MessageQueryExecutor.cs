using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryExecutor
    {
        // Publish Message
        Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessages(IGetMessagesParameters parameters, TwitterRequest request);
    }

    public class MessageQueryExecutor : IMessageQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly JsonContentFactory _jsonContentFactory;
        private readonly IMessageQueryGenerator _messageQueryGenerator;

        public MessageQueryExecutor(
            ITwitterAccessor twitterAccessor,
            JsonContentFactory jsonContentFactory,
            IMessageQueryGenerator messageQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _jsonContentFactory = jsonContentFactory;
            _messageQueryGenerator = messageQueryGenerator;
        }

        public Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request)
        {
            var requestWithPayload = _messageQueryGenerator.GetPublishMessageQuery(parameters);

            request.Query.Url = requestWithPayload.Url;
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.HttpContent = requestWithPayload.Content;

            return _twitterAccessor.ExecuteRequest<ICreateMessageDTO>(request);
        }

        public Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _messageQueryGenerator.GetDestroyMessageQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _messageQueryGenerator.GetMessageQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IGetMessageDTO>(request);
        }

        public Task<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessages(IGetMessagesParameters parameters, TwitterRequest request)
        {
            request.Query.Url = _messageQueryGenerator.GetMessagesQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IMessageCursorQueryResultDTO>(request);
        }
    }
}