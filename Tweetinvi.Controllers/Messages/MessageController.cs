using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

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

        public IEnumerable<IMessage> GetLatestMessagesReceived(IMessageGetLatestsReceivedRequestParameters messageGetLatestsReceivedRequestParameters)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesReceived(messageGetLatestsReceivedRequestParameters);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesSent(maximumMessages);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(IMessageGetLatestsSentRequestParameters messageGetLatestsSentRequestParameters)
        {
            var messagesDTO = _messageQueryExecutor.GetLatestMessagesSent(messageGetLatestsSentRequestParameters);
            return _messageFactory.GenerateMessagesFromMessagesDTO(messagesDTO);
        }

        // Publish Message
        public IMessage PublishMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            return PublishMessage(message.MessageDTO);
        }

        public IMessage PublishMessage(IMessageDTO messageDTO)
        {
            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(messageDTO);
            return _messageFactory.GenerateMessageFromMessageDTO(publishedMessageDTO);
        }

        public IMessage PublishMessage(string text, IUser targetUser)
        {
            if (targetUser == null)
            {
                throw new ArgumentException("Target user cannot be null");
            }

            return PublishMessage(text, targetUser.UserDTO);
        }

        public IMessage PublishMessage(string text, IUserIdentifier targetUserDTO)
        {
            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(text, targetUserDTO);
            return _messageFactory.GenerateMessageFromMessageDTO(publishedMessageDTO);
        }

        public IMessage PublishMessage(string text, long targetUserId)
        {
            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(text, targetUserId);
            return _messageFactory.GenerateMessageFromMessageDTO(publishedMessageDTO);
        }

        public IMessage PublishMessage(string text, string targetUserScreenName)
        {
            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(text, targetUserScreenName);
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