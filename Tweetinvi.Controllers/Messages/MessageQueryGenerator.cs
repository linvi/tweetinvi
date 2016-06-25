using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryGenerator
    {
        // Get messages
        string GetLatestMessagesReceivedQuery(IMessagesReceivedParameters queryParameters);
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
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public MessageQueryGenerator(
            IMessageQueryValidator messageQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _messageQueryValidator = messageQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        // Get collection of messages

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
            _messageQueryValidator.ThrowIfMessageCannotBePublished(parameters);

            var messageText = parameters.Text;
            var recipient = parameters.Recipient;

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(recipient);

            var query = string.Format(Resources.Message_NewMessage, _twitterStringFormatter.TwitterEncode(messageText), identifierParameter);
            query += _queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters);

            return query;
        }

        // Destroy Message
        public string GetDestroyMessageQuery(IMessageDTO messageDTO)
        {
            _messageQueryValidator.ThrowIfMessageCannotBeDestroyed(messageDTO);
            return GetDestroyMessageQuery(messageDTO.Id);
        }

        public string GetDestroyMessageQuery(long messageId)
        {
            _messageQueryValidator.ThrowIfMessageCannotBeDestroyed(messageId);
            return string.Format(Resources.Message_DestroyMessage, messageId);
        }
    }
}