using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryGenerator
    {
        // Get messages
        string GetLatestMessagesReceivedQuery(int maximumMessages);
        string GetLatestMessagesReceivedQuery(IMessagesReceivedParameters queryParameters);

        string GetLatestMessagesSentQuery(int maximumMessages);
        string GetLatestMessagesSentQuery(IMessagesSentParameters queryParameters);

        // Publish Message
        string GetPublishMessageQuery(IPublishMessageParameters parameters);

        // Detroy Message
        string GetDestroyMessageQuery(IMessageDTO messageDTO);
        string GetDestroyMessageQuery(long messageId);
    }

    public class MessageQueryGenerator : IMessageQueryGenerator
    {
        private readonly IMessageQueryValidator _messageQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public MessageQueryGenerator(
            IMessageQueryValidator messageQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            IUserQueryValidator userQueryValidator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _messageQueryValidator = messageQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
            _userQueryValidator = userQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        // Get collection of messages
        public string GetLatestMessagesReceivedQuery(int maximumMessages)
        {
            var parameter = new MessagesReceivedParameters
            {
                MaximumNumberOfMessagesToRetrieve = maximumMessages
            };

            return GetLatestMessagesReceivedQuery(parameter);
        }

        public string GetLatestMessagesReceivedQuery(IMessagesReceivedParameters queryParameters)
        {
            var query = new StringBuilder(string.Format(Resources.Message_GetMessagesReceived, queryParameters.MaximumNumberOfMessagesToRetrieve));
            
            query.Append(_queryParameterGenerator.GenerateMaxIdParameter(queryParameters.MaxId));
            query.Append(_queryParameterGenerator.GenerateSinceIdParameter(queryParameters.SinceId));
            query.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(queryParameters.IncludeEntities));
            query.Append(_queryParameterGenerator.GenerateSkipStatusParameter(queryParameters.SkipStatus));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(queryParameters.FormattedCustomQueryParameters));
            query.AddParameterToQuery("full_text", queryParameters.FullText);

            return query.ToString();
        }

        public string GetLatestMessagesSentQuery(int maximumMessages)
        {
            var parameter = new MessagesSentParameters
            {
                MaximumNumberOfMessagesToRetrieve = maximumMessages
            };

            return GetLatestMessagesSentQuery(parameter);
        }

        public string GetLatestMessagesSentQuery(IMessagesSentParameters queryParameters)
        {
            var query = new StringBuilder(string.Format(Resources.Message_GetMessagesSent, queryParameters.MaximumNumberOfMessagesToRetrieve));

            query.Append(_queryParameterGenerator.GenerateMaxIdParameter(queryParameters.MaxId));
            query.Append(_queryParameterGenerator.GenerateSinceIdParameter(queryParameters.SinceId));
            query.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(queryParameters.IncludeEntities));
            query.Append(_queryParameterGenerator.GeneratePageNumberParameter(queryParameters.PageNumber));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(queryParameters.FormattedCustomQueryParameters));
            query.AddParameterToQuery("full_text", queryParameters.FullText);

            return query.ToString();
        }

        // Publish Message
        public string GetPublishMessageQuery(IPublishMessageParameters parameters)
        {
            var messageText = parameters.Text;
            var recipient = parameters.Recipient;

            if (!_messageQueryValidator.IsMessageTextValid(messageText) || !_userQueryValidator.CanUserBeIdentified(recipient))
            {
                return null;
            }

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(recipient);

            var query = GetPublishMessageFormattedQuery(messageText, identifierParameter);

            query += _queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters);

            return query;
        }

        private string GetPublishMessageFormattedQuery(string message, string userIdentifier)
        {
            return string.Format(Resources.Message_NewMessage, _twitterStringFormatter.TwitterEncode(message), userIdentifier);
        }

        // Destroy Message
        public string GetDestroyMessageQuery(IMessageDTO messageDTO)
        {
            if (!_messageQueryValidator.CanMessageDTOBeDestroyed(messageDTO))
            {
                return null;
            }

            return GetDestroyMessageQuery(messageDTO.Id);
        }

        public string GetDestroyMessageQuery(long messageId)
        {
            if (!_messageQueryValidator.IsMessageIdValid(messageId))
            {
                return null;
            }

            return string.Format(Resources.Message_DestroyMessage, messageId);
        }
    }
}