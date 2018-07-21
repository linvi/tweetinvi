using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public class MessageController : IMessageController
    {
        private readonly IMessageQueryExecutor _messageQueryExecutor;
        private readonly IMessageFactory _messageFactory;
        private readonly IFactory<IGetMessagesParameters> _getMessagesParametersFactory;

        public MessageController(
            IMessageQueryExecutor messageQueryExecutor,
            IMessageFactory messageFactory,
            IFactory<IGetMessagesParameters> getMessagesParametersFactory)
        {
            _messageQueryExecutor = messageQueryExecutor;
            _messageFactory = messageFactory;
            _getMessagesParametersFactory = getMessagesParametersFactory;
        }

        public IEnumerable<IMessage> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return GetLatestMessages(count, out _);
        }

        public IEnumerable<IMessage> GetLatestMessages(int count, out string cursor)
        {
            var parameters = new GetMessagesParameters()
            {
                Count = count
            };

            return GetLatestMessages(parameters, out cursor);
        }

        public IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters parameters)
        {
            return GetLatestMessages(parameters, out _);
        }

        public IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters parameters, out string cursor)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            // If we've been asked to fetch more than the maximum number of results that Twitter will return from
            //  a single API request, we need to break this up into multiple requests
            if (parameters.Count > TweetinviConsts.MESSAGE_GET_COUNT)
            {
                // Firstly, run a request for the maximum number of messages from the supplied cursor
                //  Note: copy the request rather than just updating the count in case the caller intends to
                //  reuse the parameters object for future requests
                var thisReqParams = _getMessagesParametersFactory.Create();
                thisReqParams.Count = TweetinviConsts.MESSAGE_GET_COUNT;
                thisReqParams.Cursor = parameters.Cursor;

                IEnumerable<IMessage> thisReqResults = GetLatestMessages(thisReqParams, out cursor);

                // If there are more messages still available to be fetched from Twitter
                if (cursor != null)
                {
                    // Build & run the next request
                    var nextReqParams = _getMessagesParametersFactory.Create();
                    nextReqParams.Count = parameters.Count - thisReqParams.Count;
                    nextReqParams.Cursor = cursor;

                    IEnumerable<IMessage> nextReqResults = GetLatestMessages(nextReqParams, out cursor);

                    // Combine the results, latest first & return them
                    return thisReqResults.Concat(nextReqResults);
                }
                // Otherwise, just return these results
                return thisReqResults;
            }
            else // Otherwise just run the request as-is against the Twitter API
            {
                var getMessagesDTO = _messageQueryExecutor.GetLatestMessages(parameters);

                if (getMessagesDTO == null)
                {
                    cursor = null;
                    return null;
                }

                cursor = getMessagesDTO.NextCursor;
                return _messageFactory.GenerateMessageFromGetMessagesDTO(getMessagesDTO);
            }
        }

        // Publish Message
        public IMessage PublishMessage(string text, long recipientId)
        {
            var queryParameters = new PublishMessageParameters(text, recipientId);
            return PublishMessage(queryParameters);
        }

        public IMessage PublishMessage(IPublishMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Parameters cannot be null.");
            }

            var publishedMessageDTO = _messageQueryExecutor.PublishMessage(parameters);
            return _messageFactory.GenerateMessageFromCreateMessageDTO(publishedMessageDTO);
        }

        // Destroy Message
        public bool DestroyMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null");
            }

            return DestroyMessage(message.EventDTO);
        }

        public bool DestroyMessage(IEventDTO eventDTO)
        {
            if (eventDTO == null)
            {
                throw new ArgumentNullException(nameof(eventDTO));
            }

            eventDTO.MessageCreate.IsDestroyed = _messageQueryExecutor.DestroyMessage(eventDTO);
            return eventDTO.MessageCreate.IsDestroyed;
        }

        public bool DestroyMessage(long messageId)
        {
            return _messageQueryExecutor.DestroyMessage(messageId);
        }
    }
}