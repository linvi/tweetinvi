using System;
using System.Linq;
using Tweetinvi.Core.Core.Helpers;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryValidator
    {
        bool IsMessageTextValid(string message);

        void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters);
        void ThrowIfMessageCannotBeDestroyed(IEventDTO messageEvent);
        void ThrowIfMessageCannotBeDestroyed(long messageId);
    }

    public class MessageQueryValidator : IMessageQueryValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public MessageQueryValidator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public bool IsMessageTextValid(string message)
        {
            return !string.IsNullOrEmpty(message);
        }

        public void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Publish message parameters cannot be null.");
            }
            if (!IsMessageTextValid(parameters.Text))
            {
                throw new ArgumentException("Message text is not valid.");
            }
            if (!_userQueryValidator.IsUserIdValid(parameters.RecipientId))
            {
                throw new ArgumentException("Recipient User ID is not valid");
            }

            // If quick reply options are specified, validate them
            if (parameters.QuickReplyOptions != null && parameters.QuickReplyOptions.Length > 0)
            {
                if (parameters.QuickReplyOptions.Length > TweetinviConsts.MESSAGE_QUICK_REPLY_MAX_OPTIONS)
                {
                    throw new ArgumentException("There are too many Quick Reply Options. You can only have up to " +
                                                TweetinviConsts.MESSAGE_QUICK_REPLY_MAX_OPTIONS);
                }

                var hasDescription = parameters.QuickReplyOptions[0].Description != null;
                foreach (var o in parameters.QuickReplyOptions)
                {
                    // If one option has a description, then they all must
                    //  https://developer.twitter.com/en/docs/direct-messages/quick-replies/api-reference/options
                    if ((hasDescription && o.Description == null) || (!hasDescription && o.Description != null))
                    {
                        throw new ArgumentException("If one Quick Reply Option has a description, then they all must");
                    }

                    if (o.Label == null)
                    {
                        throw new ArgumentException("Quick Reply Option Label is a required field");
                    }
                    if (o.Label.UTF32Length() > TweetinviConsts.MESSAGE_QUICK_REPLY_LABEL_MAX_LENGTH)
                    {
                        throw new ArgumentException("Quick Reply Option Label too long. Max length is " +
                                                    TweetinviConsts.MESSAGE_QUICK_REPLY_LABEL_MAX_LENGTH);
                    }

                    if (o.Description != null && o.Description.UTF32Length() >
                        TweetinviConsts.MESSAGE_QUICK_REPLY_DESCRIPTION_MAX_LENGTH)
                    {
                        throw new ArgumentException("Quick Reply Option Description too long. Max length is " +
                                                    TweetinviConsts.MESSAGE_QUICK_REPLY_DESCRIPTION_MAX_LENGTH);
                    }

                    if (o.Metadata != null && o.Metadata.UTF32Length() >
                        TweetinviConsts.MESSAGE_QUICK_REPLY_METADATA_MAX_LENGTH)
                    {
                        throw new ArgumentException("Quick Reply Option Metadata too long. Max length is " +
                                                    TweetinviConsts.MESSAGE_QUICK_REPLY_METADATA_MAX_LENGTH);
                    }
                }
            }
        }

        public void ThrowIfMessageCannotBeDestroyed(IEventDTO messageEvent)
        {
            if (messageEvent == null)
            {
                throw new ArgumentNullException("Message parameters cannot be null.");
            }
            if(messageEvent.Type != EventType.MessageCreate)
            {
                throw new ArgumentException("Event must represent a message", nameof(messageEvent));
            }

            if (messageEvent.MessageCreate.IsDestroyed)
            {
                throw new ArgumentException("Message already destroyed.");
            }
        }

        public void ThrowIfMessageCannotBeDestroyed(long messageId)
        {
            if (messageId == TweetinviSettings.DEFAULT_ID)
            {
                throw new ArgumentException("Message Id must be set.");
            }
        }
    }
}