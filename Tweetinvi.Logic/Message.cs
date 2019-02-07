using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public class Message : IMessage
    {
        private readonly IMessageController _messageController;

        private IApp _app;

        private readonly ITaskFactory _taskFactory;

        private bool _mergedMediaIntoEntities = false;

        public Message(
            IMessageController messageController,
            IEventDTO eventDTO,
            IApp app,
            ITaskFactory taskFactory)
        {
            _messageController = messageController;
            EventDTO = eventDTO;
            _app = app;
            _taskFactory = taskFactory;
        }

        // Properties
        public IEventDTO EventDTO { get; }

        public IApp App
        {
            get => _app;
            set
            {
                // If the app is already set, it cannot be changed.
                //  The set option here is only to allow users to set the app for messages received
                //  in response to a create request, where Twitter doesn't return the app.
                if (_app != null)
                {
                    throw new InvalidOperationException("Cannot set the app on a message if it is already set");
                }

                _app = value;
            }
        }

        public long Id => EventDTO.Id;

        public DateTime CreatedAt => EventDTO.CreatedAt;

        public long SenderId => EventDTO.MessageCreate.SenderId;

        public long RecipientId => EventDTO.MessageCreate.Target.RecipientId;

        public IMessageEntities Entities
        {
            get
            {
                // Note: the following updates the underlying DTO and makes it slightly different
                //  to what was actually returned from Twitter. This is so that DM entities mimic
                //  Tweet entities, with media included.
                //  This shouldn't cause any issue, but if the DTO ever needed to be maintained exactly as received
                //  then entities needs to be copied before the media is added to it.
                var entities = EventDTO.MessageCreate.MessageData.Entities;
                if (!_mergedMediaIntoEntities)
                {
                    entities.Medias = new List<IMediaEntity>();
                    if (AttachedMedia != null)
                    {
                        entities.Medias.Add(AttachedMedia);
                    }

                    _mergedMediaIntoEntities = true;
                }

                return entities;
            }
        }

        public string Text => EventDTO.MessageCreate.MessageData.Text;

        public bool IsDestroyed => EventDTO.MessageCreate.IsDestroyed;

        public long? InitiatedViaTweetId => EventDTO.InitiatedVia?.TweetId;

        public long? InitiatedViaWelcomeMessageId => EventDTO.InitiatedVia?.WelcomeMessageId;

        public IQuickReplyResponse QuickReplyResponse => EventDTO.MessageCreate.MessageData.QuickReplyResponse;

        public IMediaEntity AttachedMedia => EventDTO.MessageCreate.MessageData.Attachment?.Media;

        // Destroy
        public bool Destroy()
        {
            return _messageController.DestroyMessage(EventDTO);
        }

        public bool Equals(IMessage other)
        {
            bool result = 
                Id == other.Id && 
                Text == other.Text &&
                SenderId == other.SenderId &&
                RecipientId == other.RecipientId;

            return result;
        }

        #region Async

        public async Task<bool> DestroyAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(Destroy);
        } 

        #endregion

        public override string ToString()
        {
            return Text;
        }
    }
}