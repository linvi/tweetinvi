using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Events;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.DTO.ActivityStream;

namespace Tweetinvi.Streams.Helpers
{
    public interface IAccountActivityConversationEventExtractor
    {
        T[] GetMessageConversationsEvents<T>(string eventName, JObject jsonObjectEvent, Func<ActivityStreamDirectMessageConversationEventDTO, T> ctor) where T : MessageConversationEventArgs;
    }

    public class AccountActivityConversationEventExtractor : IAccountActivityConversationEventExtractor
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IUserFactory _userFactory;

        public AccountActivityConversationEventExtractor(
            IJsonObjectConverter jsonObjectConverter,
            IUserFactory userFactory)
        {
            _jsonObjectConverter = jsonObjectConverter;
            _userFactory = userFactory;
        }

        public T[] GetMessageConversationsEvents<T>(string eventName, JObject jsonObjectEvent, Func<ActivityStreamDirectMessageConversationEventDTO, T> ctor) where T : MessageConversationEventArgs
        {
            var messageIndicateUserTypingMessageEvent = jsonObjectEvent[eventName];
            var users = jsonObjectEvent["users"].ToObject<Dictionary<long, UserDTO>>();

            var json = messageIndicateUserTypingMessageEvent.ToString();
            var messageIndicateUserTypingMessageEventDTOs =
                _jsonObjectConverter.DeserializeObject<ActivityStreamDirectMessageConversationEventDTO[]>(json);

            return messageIndicateUserTypingMessageEventDTOs.Select(
                messageIndicateUserTypingMessageEventDTO =>
                {
                    var userIsTypingMessageEventArgs = ctor(messageIndicateUserTypingMessageEventDTO);

                    userIsTypingMessageEventArgs.SenderId = messageIndicateUserTypingMessageEventDTO.SenderId;
                    userIsTypingMessageEventArgs.RecipientId = messageIndicateUserTypingMessageEventDTO.Target.RecipientId;

                    if (users.TryGetValue(messageIndicateUserTypingMessageEventDTO.SenderId, out var senderDTO))
                    {
                        userIsTypingMessageEventArgs.Sender = _userFactory.GenerateUserFromDTO(senderDTO);
                    }

                    if (users.TryGetValue(messageIndicateUserTypingMessageEventDTO.Target.RecipientId, out var recipientDTO))
                    {
                        userIsTypingMessageEventArgs.Recipient = _userFactory.GenerateUserFromDTO(recipientDTO);
                    }

                    return userIsTypingMessageEventArgs;
                }).ToArray();
        }
    }
}
