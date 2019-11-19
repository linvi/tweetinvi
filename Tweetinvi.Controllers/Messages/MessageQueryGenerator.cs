using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryGenerator
    {
        // Get messages
        string GetLatestMessagesQuery(IGetMessagesParameters queryParameters);

        // Publish Message
        string GetPublishMessageQuery(IPublishMessageParameters parameters);
        ICreateMessageDTO GetPublishMessageBody(IPublishMessageParameters parameters);

        // Detroy Message
        string GetDestroyMessageQuery(IMessageEventDTO messageDTO);
        string GetDestroyMessageQuery(long messageId);
    }

    public class MessageQueryGenerator : IMessageQueryGenerator
    {
        private readonly IMessageQueryValidator _messageQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IFactory<ICreateMessageDTO> _createMessageDTOFactory;
        private readonly IFactory<IMessageEventDTO> _eventDTOFactory;
        private readonly IFactory<IMessageCreateDTO> _messageCreateDTOFactory;
        private readonly IFactory<IMessageCreateTargetDTO> _messageCreateTargetDTOFactory;
        private readonly IFactory<IMessageDataDTO> _messageDataDTOFactory;
        private readonly IFactory<IAttachmentDTO> _attachmentDTOFactory;
        private readonly IFactory<IMediaEntity> _mediaEntityFactory;
        private readonly IFactory<IQuickReplyDTO> _quickReplyDTOFactory;

        public MessageQueryGenerator(
            IMessageQueryValidator messageQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            IFactory<ICreateMessageDTO> createMessageDTOFactory,
            IFactory<IMessageEventDTO> eventDTOFactory,
            IFactory<IMessageCreateDTO> messageCreateDTOFactory,
            IFactory<IMessageCreateTargetDTO> messageCreateTargetDTOFactory,
            IFactory<IMessageDataDTO> messageDataDTOFactory,
            IFactory<IAttachmentDTO> attachmentDTOFactory,
            IFactory<IMediaEntity> mediaEntityFactory,
            IFactory<IQuickReplyDTO> quickReplyDTOFactory)
        {
            _messageQueryValidator = messageQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _createMessageDTOFactory = createMessageDTOFactory;
            _eventDTOFactory = eventDTOFactory;
            _messageCreateDTOFactory = messageCreateDTOFactory;
            _messageCreateTargetDTOFactory = messageCreateTargetDTOFactory;
            _messageDataDTOFactory = messageDataDTOFactory;
            _attachmentDTOFactory = attachmentDTOFactory;
            _mediaEntityFactory = mediaEntityFactory;
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
        public string GetPublishMessageQuery(IPublishMessageParameters parameters)
        {
            _messageQueryValidator.ThrowIfMessageCannotBePublished(parameters);

            var query = Resources.Message_NewMessage;
            query += _queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters, false);

            return query;
        }

        public ICreateMessageDTO GetPublishMessageBody(IPublishMessageParameters parameters)
        {
            ICreateMessageDTO createMessageDTO = _createMessageDTOFactory.Create();
            createMessageDTO.MessageEvent = _eventDTOFactory.Create();
            createMessageDTO.MessageEvent.Type = EventType.MessageCreate;
            createMessageDTO.MessageEvent.MessageCreate = _messageCreateDTOFactory.Create();
            createMessageDTO.MessageEvent.MessageCreate.Target = _messageCreateTargetDTOFactory.Create();
            createMessageDTO.MessageEvent.MessageCreate.Target.RecipientId = parameters.RecipientId;
            createMessageDTO.MessageEvent.MessageCreate.MessageData = _messageDataDTOFactory.Create();
            createMessageDTO.MessageEvent.MessageCreate.MessageData.Text = parameters.Text;

            // If there is media attached, include it
            if (parameters.AttachmentMediaId != null)
            {
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment = _attachmentDTOFactory.Create();
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment.Type = AttachmentType.Media;
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment.Media = _mediaEntityFactory.Create();
                createMessageDTO.MessageEvent.MessageCreate.MessageData.Attachment.Media.Id = parameters.AttachmentMediaId;
            }

            // If there are quick reply options, include them
            if (parameters.QuickReplyOptions != null && parameters.QuickReplyOptions.Length > 0)
            {
                createMessageDTO.MessageEvent.MessageCreate.MessageData.QuickReply = _quickReplyDTOFactory.Create();
                createMessageDTO.MessageEvent.MessageCreate.MessageData.QuickReply.Type = QuickReplyType.Options;
                createMessageDTO.MessageEvent.MessageCreate.MessageData.QuickReply.Options = parameters.QuickReplyOptions;
            }

            return createMessageDTO;
        }

        // Destroy Message
        public string GetDestroyMessageQuery(IMessageEventDTO messageDTO)
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