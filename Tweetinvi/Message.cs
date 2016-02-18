using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class Message
    {
        private static IMessageFactory _messageFactory;

        /// <summary>
        /// Factory used to create Messages
        /// </summary>
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

        /// <summary>
        /// Controller handling any Message request
        /// </summary>
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

        private static IFactory<IMessagesReceivedParameters> _messageGetLatestsReceivedRequestParametersFactory;
        private static IFactory<IMessagesSentParameters> _messageGetLatestsSentRequestParametersFactory;
        private static IFactory<IPublishMessageParameters> _messagePublishParametersFactory;

        static Message()
        {
            Initialize();

            _messageGetLatestsReceivedRequestParametersFactory = TweetinviContainer.Resolve<IFactory<IMessagesReceivedParameters>>();
            _messageGetLatestsSentRequestParametersFactory = TweetinviContainer.Resolve<IFactory<IMessagesSentParameters>>();
            _messagePublishParametersFactory = TweetinviContainer.Resolve<IFactory<IPublishMessageParameters>>();
        }

        private static void Initialize()
        {
            _messageFactory = TweetinviContainer.Resolve<IMessageFactory>();
            _messageController = TweetinviContainer.Resolve<IMessageController>();
        }

        // Factory

        /// <summary>
        /// Get an existing message from its id
        /// </summary>
        public static IMessage GetExistingMessage(long messageId)
        {
            return MessageFactory.GetExistingMessage(messageId);
        }

        // Controller

        /// <summary>
        /// Get the latest messages received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessagesReceived(maximumMessages);
        }

        /// <summary>
        /// Get the latest messages received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessagesReceived(IMessagesReceivedParameters messagesReceivedParameters)
        {
            return MessageController.GetLatestMessagesReceived(messagesReceivedParameters);
        }

        /// <summary>
        /// Get the latest messages sent
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessagesSent(maximumMessages);
        }

        /// <summary>
        /// Get the latest messages sent
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessagesSent(IMessagesSentParameters messagesSentParameters)
        {
            return MessageController.GetLatestMessagesSent(messagesSentParameters);
        }

        /// <summary>
        /// Publish a message
        /// </summary>
        public static IMessage PublishMessage(string text, IUser recipient)
        {
            return MessageController.PublishMessage(text, recipient);
        }

        /// <summary>
        /// Publish a message
        /// </summary>
        public static IMessage PublishMessage(string text, IUserIdentifier recipient)
        {
            return MessageController.PublishMessage(text, recipient);
        }

        /// <summary>
        /// Publish a message
        /// </summary>
        public static IMessage PublishMessage(string text, long recipientId)
        {
            return MessageController.PublishMessage(text, recipientId);
        }

        /// <summary>
        /// Publish a message
        /// </summary>
        public static IMessage PublishMessage(string text, string recipientScreenName)
        {
            return MessageController.PublishMessage(text, recipientScreenName);
        }

        /// <summary>
        /// Publish a message
        /// </summary>
        public static IMessage PublishMessage(IPublishMessageParameters parameters)
        {
            return MessageController.PublishMessage(parameters);
        }

        /// <summary>
        /// Destroy a message
        /// </summary>
        public static bool DestroyMessage(IMessage message)
        {
            return MessageController.DestroyMessage(message);
        }

        /// <summary>
        /// Destroy a message
        /// </summary>
        public static bool DestroyMessage(IMessageDTO messageDTO)
        {
            return MessageController.DestroyMessage(messageDTO);
        }

        /// <summary>
        /// Destroy a message
        /// </summary>
        public static bool DestroyMessage(long messageId)
        {
            return MessageController.DestroyMessage(messageId);
        }

        // Parameters
        public static IMessagesReceivedParameters CreateGetLatestsReceivedRequestParameter(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = _messageGetLatestsReceivedRequestParametersFactory.Create();
            parameter.MaximumNumberOfMessagesToRetrieve = maximumMessages;
            return parameter;
        }

        public static IMessagesSentParameters CreateGetLatestsSentRequestParameter(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = _messageGetLatestsSentRequestParametersFactory.Create();
            parameter.MaximumNumberOfMessagesToRetrieve = maximumMessages;
            return parameter;
        }
    }
}