using System;
using System.Collections.Generic;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Receive and send private messages between users.
    /// </summary>
    public static class Message
    {
        [ThreadStatic]
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

        private static IFactory<IGetMessagesParameters> _getMessagesParametersFactory;
        private static IFactory<IPublishMessageParameters> _messagePublishParametersFactory;

        static Message()
        {
            Initialize();

            _getMessagesParametersFactory = TweetinviContainer.Resolve<IFactory<IGetMessagesParameters>>();
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
        /// Get the latest messages sent or received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessages(count);
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessages(int count, out string cursor)
        {
            return MessageController.GetLatestMessages(count, out cursor);
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            return MessageController.GetLatestMessages(queryParameters);
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters queryParameters, out string cursor)
        {
            return MessageController.GetLatestMessages(queryParameters, out cursor);
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
        public static bool DestroyMessage(IEventDTO eventDTO)
        {
            return MessageController.DestroyMessage(eventDTO);
        }

        /// <summary>
        /// Destroy a message
        /// </summary>
        public static bool DestroyMessage(long messageId)
        {
            return MessageController.DestroyMessage(messageId);
        }

        // Generate message from DTO
        public static IMessage GenerateMessageFromEventWithAppDTO(IEventWithAppDTO eventWithAppDTO)
        {
            return MessageFactory.GenerateMessageFromEventWithAppDTO(eventWithAppDTO);
        }

        public static IEnumerable<IMessage> GenerateMessagesFromEventWithAppDTOs(
            IEnumerable<IEventWithAppDTO> eventWithAppDTOs)
        {
            return MessageFactory.GenerateMessagesFromEventWithAppDTOs(eventWithAppDTOs);
        }

        public static string ToJson(IMessage message)
        {
            return message.ToJson();
        }

        public static string ToJson(IEventWithAppDTO eventWithAppDTO)
        {
            return eventWithAppDTO.ToJson();
        }

        public static IMessage FromJson(string json)
        {
            return MessageFactory.GenerateMessageFromJson(json);
        }
    }
}