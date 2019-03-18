using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Events;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Streams.Model.AccountActivity;

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

        public Dictionary<long, UserDTO> ExtractUserById(JObject jsonObjectEvent)
        {
            return jsonObjectEvent["users"].ToObject<Dictionary<long, UserDTO>>();
        }

        public T[] GetMessageConversationsEvents<T>(string eventName, JObject jsonObjectEvent, Func<ActivityStreamDirectMessageConversationEventDTO, T> ctor) where T : MessageConversationEventArgs
        {
            var messageIndicateUserTypingMessageEvent = jsonObjectEvent[eventName];
            var userByIds = ExtractUserById(jsonObjectEvent);

            var json = messageIndicateUserTypingMessageEvent.ToString();
            var messageIndicateUserTypingMessageEventDTOs =
                _jsonObjectConverter.DeserializeObject<ActivityStreamDirectMessageConversationEventDTO[]>(json);

            return messageIndicateUserTypingMessageEventDTOs.Select(
                messageIndicateUserTypingMessageEventDTO =>
                {
                    var userIsTypingMessageEventArgs = ctor(messageIndicateUserTypingMessageEventDTO);

                    userIsTypingMessageEventArgs.SenderId = messageIndicateUserTypingMessageEventDTO.SenderId;
                    userIsTypingMessageEventArgs.RecipientId = messageIndicateUserTypingMessageEventDTO.Target.RecipientId;

                    if (userByIds.TryGetValue(messageIndicateUserTypingMessageEventDTO.SenderId, out var senderDTO))
                    {
                        userIsTypingMessageEventArgs.Sender = _userFactory.GenerateUserFromDTO(senderDTO);
                    }

                    if (userByIds.TryGetValue(messageIndicateUserTypingMessageEventDTO.Target.RecipientId, out var recipientDTO))
                    {
                        userIsTypingMessageEventArgs.Recipient = _userFactory.GenerateUserFromDTO(recipientDTO);
                    }

                    return userIsTypingMessageEventArgs;
                }).ToArray();
        }
    }
}
