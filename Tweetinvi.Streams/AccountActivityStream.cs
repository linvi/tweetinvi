using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.DTO.ActivityStream;
using Tweetinvi.Logic.Model;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streaming;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Model.AccountActivity;

namespace Tweetinvi.Streams
{
    public class AccountActivityStream : IAccountActivityStream
    {
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetFactory _tweetFactory;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IUserFactory _userFactory;
        private readonly IMessageFactory _messageFactory;
        private readonly IAccountActivityConversationEventExtractor _accountActivityConversationEventExtractor;

        private readonly Dictionary<string, Action<string, JObject>> _events;

        public AccountActivityStream(
            IExceptionHandler exceptionHandler,
            IJObjectStaticWrapper jObjectWrapper,
            IJsonObjectConverter jsonObjectConverter,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            IMessageFactory messageFactory,
            IAccountActivityConversationEventExtractor accountActivityConversationEventExtractor)
        {
            _jObjectWrapper = jObjectWrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetFactory = tweetFactory;
            _exceptionHandler = exceptionHandler;
            _userFactory = userFactory;
            _messageFactory = messageFactory;
            _accountActivityConversationEventExtractor = accountActivityConversationEventExtractor;
            _events = new Dictionary<string, Action<string, JObject>>();

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // Tweets
            _events.Add("tweet_create_events", TryRaiseTweetCreatedEvents);
            _events.Add("tweet_delete_events", TryRaiseTweetDeletedEvents);
            _events.Add("favorite_events", TryRaiseTweetFavouritedEvents);

            // User
            _events.Add("follow_events", TryRaiseFollowedEvents);
            _events.Add("block_events", TryRaiseUserBlockedEvents);
            _events.Add("mute_events", TryRaiseUserMutedEvents);

            // App 
            _events.Add("user_event", TryRaiseUserEvent);

            // Messages
            _events.Add("direct_message_events", TryRaiseMessageEvent);
            _events.Add("direct_message_indicate_typing_events", TryRaiseIndicateUserIsTypingMessage);
            _events.Add("direct_message_mark_read_events", TryRaiseMessageReadEvent);
        }

        public long UserId { get; set; }

        // Tweets
        public EventHandler<AccountActivityTweetCreatedEventArgs> TweetCreated { get; set; }
        public EventHandler<AccountActivityTweetFavouritedEventArgs> TweetFavourited { get; set; }
        public EventHandler<AccountActivityTweetDeletedEventArgs> TweetDeleted { get; set; }

        // User Events
        public EventHandler<AccountActivityUserFollowedEventArgs> UserFollowed { get; set; }
        public EventHandler<AccountActivityUserUnfollowedEventArgs> UserUnfollowed { get; set; }

        public EventHandler<AccountActivityUserBlockedEventArgs> UserBlocked { get; set; }
        public EventHandler<AccountActivityUserUnblockedEventArgs> UserUnblocked { get; set; }
        public EventHandler<AccountActivityUserMutedEventArgs> UserMuted { get; set; }
        public EventHandler<AccountActivityUserUnmutedEventArgs> UserUnmuted { get; set; }
        public EventHandler<AccountActivityUserRevokedAppPermissionsEventArgs> UserRevokedAppPermissions { get; set; }

        // Messages
        public EventHandler<MessageEventArgs> MessageReceived { get; set; }
        public EventHandler<MessageEventArgs> MessageSent { get; set; }
        public EventHandler<UserIsTypingMessageEventArgs> UserIsTypingMessage { get; set; }
        public EventHandler<UserReadMessageConversationEventArgs> UserReadMessage { get; set; }

        public EventHandler<UnmanagedMessageReceivedEventArgs> UnmanagedEventReceived { get; set; }
        public EventHandler<JsonObjectEventArgs> JsonObjectReceived { get; set; }

        public void WebhookMessageReceived(IWebhookMessage message)
        {
            var json = message.Json;
            var jsonObjectEvent = _jObjectWrapper.GetJobjectFromJson(json);

            var jsonEventChildren = jsonObjectEvent.Children().ToArray();
            var keys = jsonEventChildren.Where(x => x.Path.EndsWith("event") || x.Path.EndsWith("events"));
            var key = keys.SingleOrDefault();

            if (key == null)
            {
                return;
            }

            this.Raise(JsonObjectReceived, new JsonObjectEventArgs(json));

            var eventName = key.Path;
            if (_events.ContainsKey(eventName))
            {
                _events[eventName].Invoke(eventName, jsonObjectEvent);
            }
            else
            {
                this.Raise(UnmanagedEventReceived, new UnmanagedMessageReceivedEventArgs(json));
            }
        }

        private void TryRaiseTweetCreatedEvents(string eventName, JObject jsonObjectEvent)
        {
            var tweetCreatedEvent = jsonObjectEvent[eventName];
            var tweetCreatedEventJson = tweetCreatedEvent.ToString();
            var tweetDTOs = _jsonObjectConverter.DeserializeObject<ITweetDTO[]>(tweetCreatedEventJson);

            tweetDTOs.ForEach(tweetDTO =>
            {
                var tweet = _tweetFactory.GenerateTweetFromDTO(tweetDTO);

                var accountActivityEvent = new AccountActivityEvent<ITweet>(tweet)
                {
                    AccountUserId = UserId,
                    EventDate = tweet.CreatedAt,
                    Json = jsonObjectEvent.ToString()
                };

                this.Raise(TweetCreated, new AccountActivityTweetCreatedEventArgs(accountActivityEvent));
            });
        }

        private void TryRaiseTweetDeletedEvents(string eventName, JObject jsonObjectEvent)
        {
            var tweetDeletedEventJToken = jsonObjectEvent[eventName];
            var tweetDeletedEventDTOs = tweetDeletedEventJToken.ToObject<AccountActivityTweetDeletedEventDTO[]>();

            tweetDeletedEventDTOs.ForEach(tweetDeletedEventDTO =>
            {
                var dateOffset = DateTimeOffset.FromUnixTimeMilliseconds(tweetDeletedEventDTO.Timestamp);

                var accountActivityEvent = new AccountActivityEvent<long>(tweetDeletedEventDTO.Status.TweetId)
                {
                    AccountUserId = UserId,
                    EventDate = dateOffset.UtcDateTime,
                    Json = jsonObjectEvent.ToString()
                };

                this.Raise(TweetDeleted, new AccountActivityTweetDeletedEventArgs(accountActivityEvent));
            });
        }

        private void TryRaiseTweetFavouritedEvents(string eventName, JObject jsonObjectEvent)
        {
            var favouriteTweetEvent = jsonObjectEvent[eventName];
            var favouritedTweetEventJson = favouriteTweetEvent.ToString();
            var favouriteEventDTOs = _jsonObjectConverter.DeserializeObject<AccountActivityFavouriteEventDTO[]>(favouritedTweetEventJson);

            favouriteEventDTOs.ForEach(favouriteEventDTO =>
            {
                var tweet = _tweetFactory.GenerateTweetFromDTO(favouriteEventDTO.FavouritedTweet);
                var user = _userFactory.GenerateUserFromDTO(favouriteEventDTO.User);

                var accountActivityEvent = new AccountActivityEvent<Tuple<ITweet, IUser>>(new Tuple<ITweet, IUser>(tweet, user))
                {
                    AccountUserId = UserId,
                    EventDate = tweet.CreatedAt,
                    Json = jsonObjectEvent.ToString(),
                };

                this.Raise(TweetFavourited, new AccountActivityTweetFavouritedEventArgs(accountActivityEvent));
            });
        }

        private void TryRaiseFollowedEvents(string eventName, JObject jsonObjectEvent)
        {
            var followEvent = jsonObjectEvent[eventName];
            var followedUsersEvents = ExtractUserToUserEventDTOs(followEvent);

            followedUsersEvents.ForEach(followedUsersEvent =>
            {
                var sourceUser = _userFactory.GenerateUserFromDTO(followedUsersEvent.Source);
                var targetUser = _userFactory.GenerateUserFromDTO(followedUsersEvent.Target);

                var timestamp = long.Parse(followedUsersEvent.CreatedTimestamp);
                var dateOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

                var accountActivityEvent = new AccountActivityEvent<Tuple<IUser, IUser>>(new Tuple<IUser, IUser>(sourceUser, targetUser))
                {
                    AccountUserId = UserId,
                    EventDate = dateOffset.UtcDateTime,
                    Json = jsonObjectEvent.ToString(),
                };

                if (followedUsersEvent.Type == "follow")
                {
                    var eventArgs = new AccountActivityUserFollowedEventArgs(accountActivityEvent);

                    this.Raise(UserFollowed, eventArgs);
                }
                else if (followedUsersEvent.Type == "unfollow")
                {
                    var eventArgs = new AccountActivityUserUnfollowedEventArgs(accountActivityEvent);

                    this.Raise(UserUnfollowed, eventArgs);
                }
                else
                {
                    this.Raise(UnmanagedEventReceived, new UnmanagedMessageReceivedEventArgs(jsonObjectEvent.ToString()));
                }
            });
        }

        private void TryRaiseUserBlockedEvents(string eventName, JObject jsonObjectEvent)
        {
            var userBlockedEvent = jsonObjectEvent[eventName];

            var blockedEventInfos = ExtractUserToUserEventDTOs(userBlockedEvent);

            blockedEventInfos.ForEach(blockedEventInfo =>
            {
                var sourceUser = _userFactory.GenerateUserFromDTO(blockedEventInfo.Source);
                var targetUser = _userFactory.GenerateUserFromDTO(blockedEventInfo.Target);

                var timestamp = long.Parse(blockedEventInfo.CreatedTimestamp);
                var dateOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

                var accountActivityEvent = new AccountActivityEvent<Tuple<IUser, IUser>>(new Tuple<IUser, IUser>(sourceUser, targetUser))
                {
                    AccountUserId = UserId,
                    EventDate = dateOffset.UtcDateTime,
                    Json = jsonObjectEvent.ToString(),
                };

                if (blockedEventInfo.Type == "block")
                {
                    this.Raise(UserBlocked, new AccountActivityUserBlockedEventArgs(accountActivityEvent));
                }
                else if (blockedEventInfo.Type == "unblock")
                {
                    this.Raise(UserUnblocked, new AccountActivityUserUnblockedEventArgs(accountActivityEvent));
                }
                else
                {
                    this.Raise(UnmanagedEventReceived, new UnmanagedMessageReceivedEventArgs(jsonObjectEvent.ToString()));
                }
            });
        }

        private void TryRaiseUserMutedEvents(string eventName, JObject jsonObjectEvent)
        {
            var userMutedEvent = jsonObjectEvent[eventName];

            var mutedEventInfos = ExtractUserToUserEventDTOs(userMutedEvent);

            mutedEventInfos.ForEach(mutedEventInfo =>
            {
                var sourceUser = _userFactory.GenerateUserFromDTO(mutedEventInfo.Source);
                var targetUser = _userFactory.GenerateUserFromDTO(mutedEventInfo.Target);

                var timestamp = long.Parse(mutedEventInfo.CreatedTimestamp);
                var dateOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

                var accountActivityEvent = new AccountActivityEvent<Tuple<IUser, IUser>>(new Tuple<IUser, IUser>(sourceUser, targetUser))
                {
                    AccountUserId = UserId,
                    EventDate = dateOffset.UtcDateTime,
                    Json = jsonObjectEvent.ToString(),
                };

                if (mutedEventInfo.Type == "mute")
                {
                    this.Raise(UserMuted, new AccountActivityUserMutedEventArgs(accountActivityEvent));
                }
                else if (mutedEventInfo.Type == "unmute")
                {
                    this.Raise(UserUnmuted, new AccountActivityUserUnmutedEventArgs(accountActivityEvent));
                }
                else
                {
                    this.Raise(UnmanagedEventReceived, new UnmanagedMessageReceivedEventArgs(jsonObjectEvent.ToString()));
                }
            });
        }

        private void TryRaiseUserEvent(string eventName, JObject jsonObjectEvent)
        {
            var userEvent = jsonObjectEvent[eventName];
            var eventType = userEvent.Children().First().Path;

            if (eventType == "user_event.revoke")
            {
                var userRevokedAppEventDTO = userEvent["revoke"].ToObject<ActivityStreamUserRevokedAppPermissionsDTO>();
                var userRevokedAppEventArgs = new AccountActivityUserRevokedAppPermissionsEventArgs
                {
                    UserId = userRevokedAppEventDTO.Source.UserId,
                    AppId = userRevokedAppEventDTO.Target.AppId
                };

                this.Raise(UserRevokedAppPermissions, userRevokedAppEventArgs);
            }
            else
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw new ArgumentException($"user_event received of type {eventType} is not supported.");
                }
            }
        }

        private void TryRaiseMessageEvent(string eventName, JObject jsonObjectEvent)
        {
            var messageEventDTOs = jsonObjectEvent[eventName].ToObject<EventDTO[]>();
            var apps = jsonObjectEvent["apps"]?.ToObject<Dictionary<string, App>>() ?? new Dictionary<string, App>();

            messageEventDTOs.ForEach(messageEventDTO =>
            {
                App app = null;

                if (messageEventDTO.MessageCreate.SourceAppId != null)
                {
                    apps.TryGetValue(messageEventDTO.MessageCreate.SourceAppId.ToString(), out app);
                }

                var message = _messageFactory.GenerateMessageFromEventDTO(messageEventDTO, app);

                if (message.SenderId == UserId)
                {
                    this.Raise(MessageSent, new MessageEventArgs(message));
                }
                else
                {
                    this.Raise(MessageReceived, new MessageEventArgs(message));
                }
            });
        }

        private void TryRaiseIndicateUserIsTypingMessage(string eventName, JObject jsonObjectEvent)
        {
            var userIsTypingMessageEventsArgs = _accountActivityConversationEventExtractor.GetMessageConversationsEvents(
                eventName,
                jsonObjectEvent,
                x => new UserIsTypingMessageEventArgs());

            userIsTypingMessageEventsArgs.ForEach(x =>
            {
                this.Raise(UserIsTypingMessage, x);
            });
        }

        private void TryRaiseMessageReadEvent(string eventName, JObject jsonObjectEvent)
        {
            var messageReadEventArgs = _accountActivityConversationEventExtractor.GetMessageConversationsEvents(eventName, jsonObjectEvent, dto =>
            {
                return new UserReadMessageConversationEventArgs
                {
                    LastReadEventId = dto.LastReadEventId
                };
            });

            messageReadEventArgs.ForEach(x =>
            {
                this.Raise(UserReadMessage, x);
            });
        }

        private IUser[] GetEventTargetUsers(JToken userToUserEvent)
        {
            var userToUserEventDTO = ExtractUserToUserEventDTOs(userToUserEvent);
            return GetEventTargetUsers(userToUserEventDTO);
        }

        private AccountActivityUserToUserEventDTO[] ExtractUserToUserEventDTOs(JToken userToUserEvent)
        {
            var userToUserEventJson = userToUserEvent.ToString();
            return ExtractUserToUserEventDTOs(userToUserEventJson);
        }

        private AccountActivityUserToUserEventDTO[] ExtractUserToUserEventDTOs(string userToUserEventJson)
        {
            var userToUserEventDTO = _jsonObjectConverter.DeserializeObject<AccountActivityUserToUserEventDTO[]>(userToUserEventJson);
            return userToUserEventDTO;
        }

        private IUser[] GetEventTargetUsers(AccountActivityUserToUserEventDTO[] userToUserEventDTO)
        {
            var userEvent = GetTargetUsersFromUserToUserEvent(userToUserEventDTO);

            return userEvent;
        }

        private IUser[] GetTargetUsersFromUserToUserEvent(AccountActivityUserToUserEventDTO[] accountActivityUserToUserEvents)
        {
            return accountActivityUserToUserEvents.Select(x =>
            {
                var source = x.Source;
                var target = x.Target;

                var targetUserDTO = source.Id == UserId ? target : source;
                var targetUser = _userFactory.GenerateUserFromDTO(targetUserDTO);
                return targetUser;
            }).ToArray();
        }
    }
}
