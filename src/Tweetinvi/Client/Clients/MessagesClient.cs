using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class MessagesClient : IMessagesClient
    {
        private readonly ITwitterClient _client;
        private readonly IMessageRequester _messageRequester;

        public MessagesClient(
            ITwitterClient client,
            IMessageRequester messageRequester)
        {
            _client = client;
            _messageRequester = messageRequester;
        }

        public IMessagesClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<IMessage> PublishMessageAsync(string text, IUserIdentifier recipient)
        {
            return PublishMessageAsync(new PublishMessageParameters(text, recipient.Id));
        }

        public Task<IMessage> PublishMessageAsync(string text, long recipientId)
        {
            return PublishMessageAsync(new PublishMessageParameters(text, recipientId));
        }

        public async Task<IMessage> PublishMessageAsync(IPublishMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.PublishMessageAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateMessage(twitterResult?.Model);
        }

        public Task<IMessage> GetMessageAsync(long messageId)
        {
            return GetMessageAsync(new GetMessageParameters(messageId));
        }

        public async Task<IMessage> GetMessageAsync(IGetMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.GetMessageAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateMessage(twitterResult?.Model);
        }

        public Task<IMessage[]> GetMessagesAsync()
        {
            return GetMessagesAsync(new GetMessagesParameters());
        }

        public async Task<IMessage[]> GetMessagesAsync(IGetMessagesParameters parameters)
        {
            var iterator = GetMessagesIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IMessage> GetMessagesIterator()
        {
            return GetMessagesIterator(new GetMessagesParameters());
        }

        public ITwitterIterator<IMessage> GetMessagesIterator(IGetMessagesParameters parameters)
        {
            var pageIterator = _messageRequester.GetMessagesIterator(parameters);

            return new TwitterIteratorProxy<ITwitterResult<IMessageCursorQueryResultDTO>, IMessage>(pageIterator,
                twitterResult =>
                {
                    var messageEventDtos = twitterResult.Model.MessageEvents;
                    var messageDtos = messageEventDtos.Select(dto =>
                    {
                        var messageDto = new MessageEventWithAppDTO
                        {
                            MessageEvent = dto
                        };

                        var appId = dto.MessageCreate.SourceAppId;
                        if (appId != null && twitterResult.Model.Apps != null && twitterResult.Model.Apps.ContainsKey(appId.Value))
                        {
                            messageDto.App = twitterResult.Model.Apps[appId.Value];
                        }

                        return messageDto as IMessageEventWithAppDTO;
                    });

                    return _client.Factories.CreateMessages(messageDtos);
                });
        }

        public Task DestroyMessageAsync(long messageId)
        {
            return DestroyMessageAsync(new DestroyMessageParameters(messageId));
        }

        public Task DestroyMessageAsync(IMessage message)
        {
            return DestroyMessageAsync(new DestroyMessageParameters(message));
        }

        public Task DestroyMessageAsync(IDeleteMessageParameters parameters)
        {
            return _messageRequester.DestroyMessageAsync(parameters);
        }
    }
}