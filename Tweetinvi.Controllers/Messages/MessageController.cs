using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public class MessageController : IMessageController
    {
        private readonly IMessageQueryExecutor _messageQueryExecutor;
        private readonly IMessageFactory _messageFactory;

        public MessageController(
            IMessageQueryExecutor messageQueryExecutor,
            IMessageFactory messageFactory)
        {
            _messageQueryExecutor = messageQueryExecutor;
            _messageFactory = messageFactory;
        }

        public IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesReceived(maximumMessages);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        public IEnumerable<IMessage> GetLatestMessagesReceived(IMessagesReceivedParameters messagesReceivedParameters)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesReceived(messagesReceivedParameters);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesSent(maximumMessages);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(IMessagesSentParameters messagesSentParameters)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesSent(messagesSentParameters);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        // Publish Message
        public IMessage PublishMessage(IMessage message)
        {
            return PublishMessage(new PublishMessageParameters(message));
        }

        public IMessage PublishMessage(IMessageDTO messageDTO)
        {
            return PublishMessage(new PublishMessageParameters(messageDTO));
        }

       public IMessage PublishMessage(string text, long recipientId)
        {
            return PublishMessage(text, new UserIdentifier(recipientId));
        }

        public IMessage PublishMessage(string text, string recipientUserName)
        {
            return PublishMessage(text, new UserIdentifier(recipientUserName));
        }

        public IMessage PublishMessage(string text, IUserIdentifier recipient)
        {
            return PublishMessage(new PublishMessageParameters(text, recipient));
        }

        public IMessage PublishMessage(IPublishMessageParameters parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("Parameter cannot be null.");
            }

            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(parameter);
            return _messageFactory.GenerateMessageFromMessageDTO(publishedMessageDTO);
        }

        // Destroy Message
        public bool DestroyMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            return DestroyMessage(message.MessageDTO);
        }

        public bool DestroyMessage(IMessageDTO messageDTO)
        {
            if (messageDTO == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            messageDTO.IsMessageDestroyed = _messageQueryExecutor.DestroyMessage(messageDTO);
            return messageDTO.IsMessageDestroyed;
        }

        public bool DestroyMessage(long messageId)
        {
            return _messageQueryExecutor.DestroyMessage(messageId);
        }
    }
}