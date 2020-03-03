using System.Net.Http;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.DTO.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public class RequestWithPayload
    {
        public string Url { get; set; }
        public HttpContent Content { get; set; }
    }

    public interface IMessageQueryGenerator
    {
        // Get messages
        string GetLatestMessagesQuery(IGetMessagesParameters queryParameters);

        // Publish Message
        RequestWithPayload GetPublishMessageQuery(IPublishMessageParameters parameters);
        string GetDestroyMessageQuery(IDeleteMessageParameters parameters);
        string GetMessageQuery(IGetMessageParameters parameters);
    }

    public class MessageQueryGenerator : IMessageQueryGenerator
    {
        private readonly JsonContentFactory _jsonContentFactory;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IFactory<IQuickReplyDTO> _quickReplyDTOFactory;

        public MessageQueryGenerator(
            JsonContentFactory jsonContentFactory,
            IQueryParameterGenerator queryParameterGenerator,
            IFactory<IQuickReplyDTO> quickReplyDTOFactory)
        {
            _jsonContentFactory = jsonContentFactory;
            _queryParameterGenerator = queryParameterGenerator;
            _quickReplyDTOFactory = quickReplyDTOFactory;
        }

        // Get collection of messages
        public string GetLatestMessagesQuery(IGetMessagesParameters queryParameters)
        {
            var query = new StringBuilder(string.Format(Resources.Message_GetMessages, queryParameters.Count));

            query.Append(_queryParameterGenerator.GenerateCursorParameter(queryParameters.Cursor));

            return query.ToString();
        }

        // Publish Message
        public RequestWithPayload GetPublishMessageQuery(IPublishMessageParameters parameters)
        {
            var query = new StringBuilder(Resources.Message_Create);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            var content = _jsonContentFactory.Create(GetPublishMessageBody(parameters));

            return new RequestWithPayload
            {
                Url = query.ToString(),
                Content = content
            };
        }

        public string GetDestroyMessageQuery(IDeleteMessageParameters parameters)
        {
            var query = new StringBuilder(Resources.Message_Destroy);
            query.AddParameterToQuery("id", parameters.MessageId);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetMessageQuery(IGetMessageParameters parameters)
        {
            var query = new StringBuilder(Resources.Message_Get);
            query.AddParameterToQuery("id", parameters.MessageId);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        private ICreateMessageDTO GetPublishMessageBody(IPublishMessageParameters parameters)
        {
            var createMessageDTO = new CreateMessageDTO
            {
                MessageEvent = new MessageEventDTO
                {
                    Type = EventType.MessageCreate,
                    MessageCreate = new MessageCreateDTO
                    {
                        Target = new MessageCreateTargetDTO
                        {
                            RecipientId = parameters.RecipientId
                        },
                        MessageData = new MessageDataDTO
                        {
                            Text = parameters.Text
                        }
                    },
                }
            };

            // If there is media attached, include it
            if (parameters.AttachmentMediaId != null)
            {
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment = new AttachmentDTO
                {
                    Type = AttachmentType.Media,
                    Media = new MediaEntity { Id = parameters.AttachmentMediaId }
                };
            }

            // If there are quick reply options, include them
            if (parameters.QuickReplyOptions != null && parameters.QuickReplyOptions.Length > 0)
            {
                createMessageDTO.MessageEvent.MessageCreate.MessageData.QuickReply = new QuickReplyDTO
                {
                    Type = QuickReplyType.Options,
                    Options = parameters.QuickReplyOptions,
                };
            }

            return createMessageDTO;
        }
    }
}