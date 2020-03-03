using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public class MessageController : IMessageController
    {
        private readonly IMessageQueryExecutor _messageQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly IFactory<IGetMessagesParameters> _getMessagesParametersFactory;

        public MessageController(
            IMessageQueryExecutor messageQueryExecutor,
            ITwitterClientFactories factories,
            IFactory<IGetMessagesParameters> getMessagesParametersFactory)
        {
            _messageQueryExecutor = messageQueryExecutor;
            _factories = factories;
            _getMessagesParametersFactory = getMessagesParametersFactory;
        }

        public Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameters = new GetMessagesParameters
            {
                Count = count
            };

            return GetLatestMessages(parameters);
        }

        public async Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(IGetMessagesParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var cursorResult = new AsyncCursorResult<IEnumerable<IMessage>>();

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

                var result = await _messageQueryExecutor.GetLatestMessages(thisReqParams);

                var cursor = result.NextCursor;

                cursorResult.Cursor = cursor;

                var messages = _factories.CreateMessages(result);

                // If there are more messages still available to be fetched from Twitter
                if (!string.IsNullOrEmpty(cursor))
                {
                    // Build & run the next request
                    var nextReqParams = _getMessagesParametersFactory.Create();
                    nextReqParams.Count = parameters.Count - thisReqParams.Count;
                    nextReqParams.Cursor = cursor;

                    var nextReqResults = await GetLatestMessages(nextReqParams);

                    cursorResult.Result = messages.Concat(nextReqResults.Result);

                    // Combine the results, latest first & return them
                    return cursorResult;
                }

                cursorResult.Result = messages;

                return cursorResult;
            }

            var getMessagesDTO = await _messageQueryExecutor.GetLatestMessages(parameters);

            if (getMessagesDTO != null)
            {
                cursorResult.Cursor = getMessagesDTO.NextCursor;
                cursorResult.Result = _factories.CreateMessages(getMessagesDTO);
            }

            return cursorResult;
        }

        public Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.PublishMessage(parameters, request);
        }

        public Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.DestroyMessage(parameters, request);
        }

        public Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request)
        {
            return _messageQueryExecutor.GetMessage(parameters, request);
        }
    }
}