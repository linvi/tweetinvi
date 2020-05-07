using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public class Message : IMessage
    {
        private IApp _app;
        private bool _mergedMediaIntoEntities;

        public Message(IMessageEventDTO messageEventDTO, IApp app, ITwitterClient client)
        {
            MessageEventDTO = messageEventDTO;
            Client = client;
            _app = app;
        }

        // Properties
        public IMessageEventDTO MessageEventDTO { get; }
        public ITwitterClient Client { get; }

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

        public long Id => MessageEventDTO.Id;

        public DateTime CreatedAt => MessageEventDTO.CreatedAt;

        public long SenderId => MessageEventDTO.MessageCreate.SenderId;

        public long RecipientId => MessageEventDTO.MessageCreate.Target.RecipientId;

        public IMessageEntities Entities
        {
            get
            {
                // Note: the following updates the underlying DTO and makes it slightly different
                //  to what was actually returned from Twitter. This is so that DM entities mimic
                //  Tweet entities, with media included.
                //  This shouldn't cause any issue, but if the DTO ever needed to be maintained exactly as received
                //  then entities needs to be copied before the media is added to it.
                var entities = MessageEventDTO.MessageCreate.MessageData.Entities;
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

        public string Text => MessageEventDTO.MessageCreate.MessageData?.Text;
        public long? InitiatedViaTweetId => MessageEventDTO.InitiatedVia?.TweetId;
        public long? InitiatedViaWelcomeMessageId => MessageEventDTO.InitiatedVia?.WelcomeMessageId;
        public IQuickReplyOption[] QuickReplyOptions => MessageEventDTO.MessageCreate.MessageData?.QuickReply?.Options;
        public IQuickReplyResponse QuickReplyResponse => MessageEventDTO.MessageCreate.MessageData?.QuickReplyResponse;
        public IMediaEntity AttachedMedia => MessageEventDTO.MessageCreate.MessageData?.Attachment?.Media;

        // Destroy
        public Task DestroyAsync()
        {
            return Client.Messages.DestroyMessageAsync(this);
        }

        public bool Equals(IMessage other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id &&
                Text == other.Text &&
                SenderId == other.SenderId &&
                RecipientId == other.RecipientId;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}