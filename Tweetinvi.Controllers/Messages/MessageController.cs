using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public class MessageController : IMessageController
    {
        private readonly IMessageQueryExecutor _messageQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly IFactory<IGetMessagesParameters> _getMessagesParametersFactory;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;

        public MessageController(
            IMessageQueryExecutor messageQueryExecutor,
            ITwitterClientFactories factories,
            IFactory<IGetMessagesParameters> getMessagesParametersFactory,
            IPageCursorIteratorFactories pageCursorIteratorFactories)
        {
            _messageQueryExecutor = messageQueryExecutor;
            _factories = factories;
            _getMessagesParametersFactory = getMessagesParametersFactory;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
        }

        public Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.PublishMessage(parameters, request);
        }

        public Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.DestroyMessage(parameters, request);
        }

        public Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.GetMessage(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessagesIterator(IGetMessagesParameters parameters, ITwitterRequest request)
        {
            return new TwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMessagesParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _messageQueryExecutor.GetMessages(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page =>
                {
                    return page.DataTransferObject.NextCursorStr == "0" || string.IsNullOrEmpty(page.DataTransferObject.NextCursorStr);
                });
        }
    }
}