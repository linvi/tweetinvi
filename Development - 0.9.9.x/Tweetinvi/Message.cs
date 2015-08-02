using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi
{
    public static class Message
    {
        [ThreadStatic]
        private static IMessageFactory _messageFactory;
        public static IMessageFactory MessageFactory
        {
            get
            {
                if (_messageFactory == null)
                {
                    Initialize();
                }

                return _messageFactory;
            }
        }

        [ThreadStatic]
        private static IMessageController _messageController;
        public static IMessageController MessageController
        {
            get
            {
                if (_messageController == null)
                {
                    Initialize();
                }

                return _messageController;
            }
        }

        private static IFactory<IMessageGetLatestsReceivedRequestParameters> _messageGetLatestsReceivedRequestParametersFactory;
        private static IFactory<IMessageGetLatestsSentRequestParameters> _messageGetLatestsSentRequestParametersFactory;

        static Message()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _messageFactory = TweetinviContainer.Resolve<IMessageFactory>();
            _messageController = TweetinviContainer.Resolve<IMessageController>();
            _messageGetLatestsReceivedRequestParametersFactory = TweetinviContainer.Resolve<IFactory<IMessageGetLatestsReceivedRequestParameters>>();
            _messageGetLatestsSentRequestParametersFactory = TweetinviContainer.Resolve<IFactory<IMessageGetLatestsSentRequestParameters>>();
        }

        // Factory
        public static IMessage GetExistingMessage(long messageId)
        {
            return MessageFactory.GetExistingMessage(messageId);
        }

        public static IMessage CreateMessage(string text, IUser recipient = null)
        {
            return MessageFactory.CreateMessage(text, recipient);
        }

        // Controller
        public static IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessagesReceived(maximumMessages);
        }

        public static IEnumerable<IMessage> GetLatestMessagesReceived(IMessageGetLatestsReceivedRequestParameters messageGetLatestsReceivedRequestParameters)
        {
            return MessageController.GetLatestMessagesReceived(messageGetLatestsReceivedRequestParameters);
        }

        public static IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessagesSent(maximumMessages);
        }

        public static IEnumerable<IMessage> GetLatestMessagesSent(IMessageGetLatestsSentRequestParameters messageGetLatestsSentRequestParameters)
        {
            return MessageController.GetLatestMessagesSent(messageGetLatestsSentRequestParameters);
        }

        public static IMessage PublishMessage(IMessage message)
        {
            return MessageController.PublishMessage(message);
        }

        public static IMessage PublishMessage(IMessageDTO messageDTO)
        {
            return MessageController.PublishMessage(messageDTO);
        }

        public static IMessage PublishMessage(string text, IUser targetUser)
        {
            return MessageController.PublishMessage(text, targetUser);
        }

        public static IMessage PublishMessage(string text, IUserIdentifier targetUserDTO)
        {
            return MessageController.PublishMessage(text, targetUserDTO);
        }

        public static IMessage PublishMessage(string text, long targetUserId)
        {
            return MessageController.PublishMessage(text, targetUserId);
        }

        public static IMessage PublishMessage(string text, string targetUserScreenName)
        {
            return MessageController.PublishMessage(text, targetUserScreenName);
        }

        public static bool DestroyMessage(IMessage message)
        {
            return MessageController.DestroyMessage(message);
        }

        public static bool DestroyMessage(IMessageDTO messageDTO)
        {
            return MessageController.DestroyMessage(messageDTO);
        }

        public static bool DestroyMessage(long messageId)
        {
            return MessageController.DestroyMessage(messageId);
        }

        // Parameters
        public static IMessageGetLatestsReceivedRequestParameters CreateGetLatestsReceivedRequestParameter(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = _messageGetLatestsReceivedRequestParametersFactory.Create();
            parameter.MaximumNumberOfMessagesToRetrieve = maximumMessages;
            return parameter;
        }

        public static IMessageGetLatestsSentRequestParameters CreateGetLatestsSentRequestParameter(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = _messageGetLatestsSentRequestParametersFactory.Create();
            parameter.MaximumNumberOfMessagesToRetrieve = maximumMessages;
            return parameter;
        }
    }
}