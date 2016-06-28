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
            var messageDTO = _messageFactoryQueryExecutor.GetExistingMessage(messageId);
            return GenerateMessageFromMessageDTO(messageDTO);
        }

        // Create Message
        public IMessage CreateMessage(string text, IUser recipient = null)
        {
            var messageDTO = _messageFactoryQueryExecutor.CreateMessage(text, recipient != null ? recipient.UserDTO : null);
            return GenerateMessageFromMessageDTO(messageDTO);
        }

        // Generate Message from DTO
        public IMessage GenerateMessageFromMessageDTO(IMessageDTO messageDTO)
        {
            if (messageDTO == null)
            {
                return null;
            }

            var messageParameter = _messageUnityFactory.GenerateParameterOverrideWrapper("messageDTO", messageDTO);
            return _messageUnityFactory.Create(messageParameter);
        }

        public IEnumerable<IMessage> GenerateMessagesFromMessagesDTO(IEnumerable<IMessageDTO> messagesDTO)
        {
            if (messagesDTO == null)
            {
                return null;
            }

            return messagesDTO.Select(GenerateMessageFromMessageDTO);
        }

        // Generate Message from Json
        public IMessage GenerateMessageFromJson(string jsonMessage)
        {
            var messageDTO = _jsonObjectConverter.DeserializeObject<IMessageDTO>(jsonMessage);
            if (messageDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateMessageFromMessageDTO(messageDTO);
        }
    }
}