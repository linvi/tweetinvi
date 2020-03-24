using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
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

        public MessageController(IMessageQueryExecutor messageQueryExecutor)
        {
            _messageQueryExecutor = messageQueryExecutor;
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