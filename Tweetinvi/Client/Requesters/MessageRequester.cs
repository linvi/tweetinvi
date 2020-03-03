using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class MessageRequester : BaseRequester, IMessageRequester
    {
        private readonly ITwitterClient _client;
        private readonly IMessageController _messageController;
        private readonly IMessagesClientParametersValidator _messagesClientParametersValidator;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public MessageRequester(
            ITwitterClient client,
            IMessageController messageController,
            IMessagesClientParametersValidator messagesClientParametersValidator,
            ITwitterResultFactory twitterResultFactory,
            ITwitterClientEvents twitterClientEvents)
            : base(client, twitterClientEvents)
        {
            _client = client;
            _messageController = messageController;
            _messagesClientParametersValidator = messagesClientParametersValidator;
            _twitterResultFactory = twitterResultFactory;
        }

        public Task<ITwitterResult<ICreateMessageDTO, IMessage>> PublishMessage(IPublishMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _messageController.PublishMessage(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _client.Factories.CreateMessage(dto));
            });
        }

        public Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _messageController.DestroyMessage(parameters, request));
        }

        public Task<ITwitterResult<IGetMessageDTO, IMessage>> GetMessage(IGetMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _messageController.GetMessage(parameters, request);
                return _twitterResultFactory.Create(twitterResult, dto => _client.Factories.CreateMessage(dto));
            });
        }
    }
}