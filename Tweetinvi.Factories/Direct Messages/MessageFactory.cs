using System.Collections.Generic;
using System.Linq;
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
        private readonly IFactory<IMessage> _messageUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public MessageFactory(
            IMessageFactoryQueryExecutor messageFactoryQueryExecutor,
            IFactory<IMessage> messageUnityFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _messageFactoryQueryExecutor = messageFactoryQueryExecutor;
            _messageUnityFactory = messageUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Get existing message
        public IMessage GetExistingMessage(long messageId)
        {
            var getMessageDTO = _messageFactoryQueryExecutor.GetExistingMessage(messageId);
            return GenerateMessageFromGetMessageDTO(getMessageDTO);
        }

        // Generate Message from DTO
        public IMessage GenerateMessageFromEventWithAppDTO(IEventWithAppDTO eventWithAppDTO)
        {
            if (eventWithAppDTO?.Event == null || eventWithAppDTO.Event.Type == EventType.MessageCreate)
            {
                return null;
            }

            return buildMessage(eventWithAppDTO.Event, eventWithAppDTO.App);
        }

        public IEnumerable<IMessage> GenerateMessagesFromEventWithAppDTOs(IEnumerable<IEventWithAppDTO> eventWithAppDTOs)
        {
            return eventWithAppDTOs?.Select(GenerateMessageFromEventWithAppDTO);
        }

        public IMessage GenerateMessageFromGetMessageDTO(IGetMessageDTO getMessageDTO)
        {
            if (getMessageDTO?.Event == null || getMessageDTO.Event.Type != EventType.MessageCreate)
            {
                return null;
            }

            var app = getMessageDTO.Apps[getMessageDTO.Event.MessageCreate.SourceAppId];

            return buildMessage(getMessageDTO.Event, app);
        }

        public IEnumerable<IMessage> GenerateMessageFromGetMessagesDTO(IGetMessagesDTO getMessagesDTO)
        {
            return getMessagesDTO?.Events?.Select(eventDTO =>
            {
                if (eventDTO.Type != EventType.MessageCreate)
                {
                    return null;
                }

                // Get the app that was used to send this message.
                //  Note that some apps could be missing from the dictionary.
                getMessagesDTO.Apps.TryGetValue(eventDTO.MessageCreate.SourceAppId, out var app);

                return buildMessage(eventDTO, app);
            });
        }

        public IMessage GenerateMessageFromCreateMessageDTO(ICreateMessageDTO createMessageDTO)
        {
            if (createMessageDTO?.Event == null || createMessageDTO.Event.Type != EventType.MessageCreate)
            {
                return null;
            }

            return buildMessage(createMessageDTO.Event, null);
        }

        // Generate Message from Json
        public IMessage GenerateMessageFromJson(string jsonMessage)
        {
            var eventWithAppDTO = _jsonObjectConverter.DeserializeObject<IEventWithAppDTO>(jsonMessage);
            if (eventWithAppDTO.Event == null || eventWithAppDTO.Event.Type != EventType.MessageCreate ||
                eventWithAppDTO.Event.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateMessageFromEventWithAppDTO(eventWithAppDTO);
        }

        private IMessage buildMessage(IEventDTO eventDTO, IApp app)
        {
            var eventParameter = _messageUnityFactory.GenerateParameterOverrideWrapper("eventDTO", eventDTO);
            var appParameter = _messageUnityFactory.GenerateParameterOverrideWrapper("app", app);
            return _messageUnityFactory.Create(eventParameter, appParameter);
        }
    }
}