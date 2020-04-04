using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
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

        public Task<IMessage> PublishMessage(string text, long recipientId)
        {
            return PublishMessage(new PublishMessageParameters(text, recipientId));
        }

        public async Task<IMessage> PublishMessage(IPublishMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.PublishMessage(parameters).ConfigureAwait(false);
            return _client.Factories.CreateMessage(twitterResult?.DataTransferObject);
        }

        public Task<IMessage> GetMessage(long messageId)
        {
            return GetMessage(new GetMessageParameters(messageId));
        }

        public async Task<IMessage> GetMessage(IGetMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.GetMessage(parameters).ConfigureAwait(false);
            return _client.Factories.CreateMessage(twitterResult?.DataTransferObject);
        }

        public Task<IMessage[]> GetMessages()
        {
            return GetMessages(new GetMessagesParameters());
        }

        public async Task<IMessage[]> GetMessages(IGetMessagesParameters parameters)
        {
            var iterator = GetMessagesIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
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
                    var messageEventDtos = twitterResult.DataTransferObject.MessageEvents;
                    var messageDtos = messageEventDtos.Select(dto =>
                    {
                        var messageDto = new MessageEventWithAppDTO
                        {
                            MessageEvent = dto
                        };

                        var appId = dto.MessageCreate.SourceAppId;
                        if (appId != null && twitterResult.DataTransferObject.Apps != null && twitterResult.DataTransferObject.Apps.ContainsKey(appId.Value))
                        {
                            messageDto.App = twitterResult.DataTransferObject.Apps[appId.Value];
                        }

                        return messageDto as IMessageEventWithAppDTO;
                    });

                    return _client.Factories.CreateMessages(messageDtos);
                });
        }

        public Task DestroyMessage(long messageId)
        {
            return DestroyMessage(new DestroyMessageParameters(messageId));
        }

        public Task DestroyMessage(IMessage message)
        {
            return DestroyMessage(new DestroyMessageParameters(message));
        }

        public Task DestroyMessage(IDeleteMessageParameters parameters)
        {
            return _messageRequester.DestroyMessage(parameters);
        }
    }
}