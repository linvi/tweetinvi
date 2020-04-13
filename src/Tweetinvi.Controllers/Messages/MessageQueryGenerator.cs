using System.Net.Http;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.DTO.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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
        RequestWithPayload GetPublishMessageQuery(IPublishMessageParameters parameters);
        string GetDestroyMessageQuery(IDeleteMessageParameters parameters);
        string GetMessageQuery(IGetMessageParameters parameters);
        string GetMessagesQuery(IGetMessagesParameters parameters);
    }

    public class MessageQueryGenerator : IMessageQueryGenerator
    {
        private readonly JsonContentFactory _jsonContentFactory;
        private readonly IQueryParameterGenerator _queryParameterGenerator;

        public MessageQueryGenerator(
            JsonContentFactory jsonContentFactory,
            IQueryParameterGenerator queryParameterGenerator)
        {
            _jsonContentFactory = jsonContentFactory;
            _queryParameterGenerator = queryParameterGenerator;
        }

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

        public string GetMessagesQuery(IGetMessagesParameters parameters)
        {
            var query = new StringBuilder(Resources.Message_GetMessages);
            _queryParameterGenerator.AppendCursorParameters(query, parameters);
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
            if (parameters.MediaId != null)
            {
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment = new AttachmentDTO
                {
                    Type = AttachmentType.Media,
                    Media = new MediaEntity { Id = parameters.MediaId }
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