using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IMessageFactoryQueryExecutor _messageFactoryQueryExecutor;
        private readonly IFactory<IMessage> _messageFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public MessageFactory(
            IMessageFactoryQueryExecutor messageFactoryQueryExecutor,
            IFactory<IMessage> messageFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _messageFactoryQueryExecutor = messageFactoryQueryExecutor;
            _messageFactory = messageFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Get existing message
        public async Task<IMessage> GetExistingMessage(long messageId)
        {
            var getMessageDTO = await _messageFactoryQueryExecutor.GetExistingMessage(messageId);
            return GenerateMessageFromGetMessageDTO(getMessageDTO);
        }

        // Generate Message from DTO
        public IMessage GenerateMessageFromEventWithAppDTO(IMessageEventWithAppDTO messageEventWithAppDTO)
        {
            if (messageEventWithAppDTO?.MessageEvent == null || messageEventWithAppDTO.MessageEvent.Type == EventType.MessageCreate)
            {
                return null;
            }

            return _buildMessage(messageEventWithAppDTO.MessageEvent, messageEventWithAppDTO.App);
        }

        public IEnumerable<IMessage> GenerateMessagesFromEventWithAppDTOs(IEnumerable<IMessageEventWithAppDTO> eventWithAppDTOs)
        {
            return eventWithAppDTOs?.Select(GenerateMessageFromEventWithAppDTO);
        }

        public IMessage GenerateMessageFromGetMessageDTO(IGetMessageDTO getMessageDTO)
        {
            if (getMessageDTO?.MessageEvent == null || getMessageDTO.MessageEvent.Type != EventType.MessageCreate)
            {
                return null;
            }

            return _buildMessage(getMessageDTO.MessageEvent, getMessageDTO.Apps);
        }

        public IEnumerable<IMessage> GenerateMessageFromGetMessagesDTO(IGetMessagesDTO getMessagesDTO)
        {
            return getMessagesDTO?.MessageEvents?.Select(eventDTO => _buildMessage(eventDTO, getMessagesDTO.Apps));
        }

        public IMessage GenerateMessageFromCreateMessageDTO(ICreateMessageDTO createMessageDTO)
        {
            if (createMessageDTO?.MessageEvent == null || createMessageDTO.MessageEvent.Type != EventType.MessageCreate)
            {
                return null;
            }

            return GenerateMessageFromEventDTO(createMessageDTO.MessageEvent);
        }

        public IMessage GenerateMessageFromEventDTO(IMessageEventDTO createMessageDTO, IApp app = null)
        {
            return _buildMessage(createMessageDTO, app);
        }

        // Generate Message from Json
        public IMessage GenerateMessageFromJson(string jsonMessage)
        {
            var eventWithAppDTO = _jsonObjectConverter.DeserializeObject<IMessageEventWithAppDTO>(jsonMessage);
            if (eventWithAppDTO.MessageEvent == null || eventWithAppDTO.MessageEvent.Type != EventType.MessageCreate ||
                eventWithAppDTO.MessageEvent.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateMessageFromEventWithAppDTO(eventWithAppDTO);
        }

        private IMessage _buildMessage(IMessageEventDTO messageEventDTO, IDictionary<long, IApp> apps)
        {
            if (messageEventDTO.Type != EventType.MessageCreate)
            {
                return null;
            }

            // Get the app that was used to send this message.
            //  Note that we don't always get the App ID.
            //  Also assume that some apps could be missing from the dictionary.
            IApp app = null;
            if (messageEventDTO.MessageCreate.SourceAppId != null)
            {
                apps.TryGetValue(messageEventDTO.MessageCreate.SourceAppId.Value, out app);
            }

            return _buildMessage(messageEventDTO, app);
        }

        private IMessage _buildMessage(IMessageEventDTO messageEventDTO, IApp app)
        {
            var eventParameter = _messageFactory.GenerateParameterOverrideWrapper("messageEventDTO", messageEventDTO);
            var appParameter = _messageFactory.GenerateParameterOverrideWrapper("app", app);

            return _messageFactory.Create(eventParameter, appParameter);
        }
    }
}