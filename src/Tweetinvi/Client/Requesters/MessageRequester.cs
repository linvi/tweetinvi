using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class MessageRequester : BaseRequester, IMessageRequester
    {
        private readonly IMessageController _messageController;
        private readonly IMessagesClientParametersValidator _messagesClientParametersValidator;

        public MessageRequester(
            ITwitterClient client,
            IMessageController messageController,
            IMessagesClientParametersValidator messagesClientParametersValidator,
            ITwitterClientEvents twitterClientEvents)
            : base(client, twitterClientEvents)
        {
            _messageController = messageController;
            _messagesClientParametersValidator = messagesClientParametersValidator;
        }

        public Task<ITwitterResult<ICreateMessageDTO>> PublishMessageAsync(IPublishMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _messageController.PublishMessageAsync(parameters, request));
        }

        public Task<ITwitterResult> DestroyMessageAsync(IDeleteMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _messageController.DestroyMessageAsync(parameters, request));
        }

        public Task<ITwitterResult<IGetMessageDTO>> GetMessageAsync(IGetMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _messageController.GetMessageAsync(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessagesIterator(IGetMessagesParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _messageController.GetMessagesIterator(parameters, request);
        }
    }
}