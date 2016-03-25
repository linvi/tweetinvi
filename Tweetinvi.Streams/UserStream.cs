using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Model;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class UserStream : TrackedStream, IUserStream
    {
        // TODO : Tweet tracking

        private readonly IMessageFactory _messageFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListFactory _twitterListFactory;
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly ITaskFactory _taskFactory;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ISingleAggregateExceptionThrower _singleAggregateExceptionThrower;

        private IAuthenticatedUser _authenticatedUser;
        private HashSet<long> _friendIds;
        private readonly Dictionary<string, Action<JObject>> _events;

        // Parameters
        private RepliesFilterType _repliesFilterType;
        private WithFilterType _withFilterType;

        public event EventHandler StreamIsReady;

        public UserStream(
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            IMessageFactory messageFactory,
            IUserFactory userFactory,
            ITwitterListFactory twitterListFactory,
            IJObjectStaticWrapper jObjectWrapper,
            IJsonObjectConverter jsonObjectConverter,
            IStreamTrackManager<ITweet> streamTrackManager,
            ISynchronousInvoker synchronousInvoker,
            ITaskFactory taskFactory,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory,
            ISingleAggregateExceptionThrower singleAggregateExceptionThrower)

            : base(
                streamTrackManager,
                jsonObjectConverter,
                jObjectWrapper,
                streamResultGenerator,
                tweetFactory,
                synchronousInvoker,
                customRequestParameters,
                twitterQueryFactory,
                singleAggregateExceptionThrower)
        {
            _messageFactory = messageFactory;
            _userFactory = userFactory;
            _twitterListFactory = twitterListFactory;
            _jObjectWrapper = jObjectWrapper;
            _taskFactory = taskFactory;
            _twitterQueryFactory = twitterQueryFactory;
            _singleAggregateExceptionThrower = singleAggregateExceptionThrower;

            _events = new Dictionary<string, Action<JObject>>();

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _events.Add("follow", TryRaiseUserFollowedEvent);
            _events.Add("favorite", TryRaiseFavouriteEvent);
            _events.Add("block", TryRaiseUserBlockedEvent);
            _events.Add("user_update", TryRaiseUserUpdatedEvent);

            _events.Add("unfollow", TryRaiseUserUnFollowedEvent);
            _events.Add("unfavorite", TryRaiseUnFavouriteEvent);
            _events.Add("unblock", TryRaiseUserUnBlockedEvent);

            _events.Add("quoted_tweet", TryRaiseQuotedTweetEvent);

            // List events
            _events.Add("list_created", TryRaiseListCreatedEvent);
            _events.Add("list_updated", TryRaiseListUpdatedEvent);
            _events.Add("list_destroyed", TryRaiseListDestroyedEvent);
            _events.Add("list_member_added", TryRaiseListMemberAddedEvent);
            _events.Add("list_member_removed", TryRaiseListMemberRemovedEvent);
            _events.Add("list_member_subscribed", TryRaiseListMemberSubscribedEvent);
            _events.Add("list_member_unsubscribed", TryRaiseListMemberUnsubscribedEvent);

            // Credentials
            _events.Add("access_revoked", TryRaiseAccessRevokedEvent);
        }

        public void StartStream()
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(() => StartStreamAsync());
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamAsync()
        {
            _authenticatedUser = await _taskFactory.ExecuteTaskAsync(() => _userFactory.GetAuthenticatedUser(Credentials));
            if (_authenticatedUser == null)
            {
                StopStream(new UserStreamFailedToInitialize("Could not receive information related with currently authenticated user."));
                return;
            }

            Func<ITwitterQuery> generateTwitterQuery = delegate
            {
                var queryBuilder = new StringBuilder(Resources.Stream_UserStream);
                AddBaseParametersToQuery(queryBuilder);

                return _twitterQueryFactory.Create(queryBuilder.ToString(), HttpMethod.GET, Credentials);
            };

            Action<string> eventReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                // We analyze the different types of message from the stream
                if (TryGetEvent(json)) return;
                if (TryGetTweet(json)) return;
                if (TryGetMessage(json)) return;
                if (TryGetWarning(json)) return;
                if (TryGetFriends(json)) return;

                TryInvokeGlobalStreamMessages(json);
            };

            await _streamResultGenerator.StartStreamAsync(eventReceived, generateTwitterQuery);
        }

        // Parameters
        protected override void AddBaseParametersToQuery(StringBuilder queryBuilder)
        {
            base.AddBaseParametersToQuery(queryBuilder);

            if (_repliesFilterType == RepliesFilterType.AllReplies)
            {
                queryBuilder.AddParameterToQuery("replies", "all");
            }

            switch (_withFilterType)
            {
                case WithFilterType.Followings:
                    queryBuilder.AddParameterToQuery("with", "followings");
                    break;
                case WithFilterType.User:
                    queryBuilder.AddParameterToQuery("with", "user");
                    break;
            }
        }

        public RepliesFilterType RepliesFilterType
        {
            get { return _repliesFilterType; }
            set { _repliesFilterType = value; }
        }

        public WithFilterType WithFilterType
        {
            get { return _withFilterType; }
            set { _withFilterType = value; }
        }

        #region User Stream Events
        // Tweets
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByMe;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByFriend;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyone;
        public event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyoneButMe;

        // Messages
        public event EventHandler<MessageEventArgs> MessageSent;
        public event EventHandler<MessageEventArgs> MessageReceived;

        // Friends
        public event EventHandler<GenericEventArgs<IEnumerable<long>>> FriendIdsReceived;

        // Follow
        public event EventHandler<UserFollowedEventArgs> FollowedUser;
        public event EventHandler<UserFollowedEventArgs> FollowedByUser;

        public event EventHandler<UserFollowedEventArgs> UnFollowedUser;

        // Favourite
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByMe;
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyone;
        public event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyoneButMe;

        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByMe;
        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyone;
        public event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyoneButMe;

        // List
        public event EventHandler<ListEventArgs> ListCreated;
        public event EventHandler<ListEventArgs> ListUpdated;
        public event EventHandler<ListEventArgs> ListDestroyed;

        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedMemberToList;
        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedToListBy;

        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedMemberFromList;
        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedFromListBy;

        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserSubscribedToListCreatedBy;
        public event EventHandler<ListUserUpdatedEventArgs> UserSubscribedToListCreatedByMe;

        public event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserUnsubscribedToListCreatedBy;
        public event EventHandler<ListUserUpdatedEventArgs> UserUnsubscribedToListCreatedByMe;

        // Block
        public event EventHandler<UserBlockedEventArgs> BlockedUser;
        public event EventHandler<UserBlockedEventArgs> UnBlockedUser;

        // Profile Updated
        public event EventHandler<AuthenticatedUserUpdatedEventArgs> AuthenticatedUserProfileUpdated;

        // Warning
        public event EventHandler<WarningTooManyFollowersEventArgs> WarningTooManyFollowersDetected;

        // Credentials
        public event EventHandler<AccessRevokedEventArgs> AccessRevoked;

        private bool TryGetEvent(string jsonEvent)
        {
            var jsonObjectEvent = _jObjectWrapper.GetJobjectFromJson(jsonEvent);
            JToken jsonEventToken;

            if (jsonObjectEvent.TryGetValue("event", out jsonEventToken))
            {
                string eventName = jsonEventToken.Value<string>();

                if (_events.ContainsKey(eventName))
                {
                    _events[eventName].Invoke(jsonObjectEvent);
                }

                return true;
            }

            return false;
        }

        // Tweet
        private bool TryGetTweet(string jsonTweet)
        {
            try
            {
                var tweet = _tweetFactory.GenerateTweetFromJson(jsonTweet);
                if (!TryRaiseTweetEvent(tweet))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool TryRaiseTweetEvent(ITweet tweet)
        {
            if (tweet == null)
            {
                return false;
            }

            var tweetReceivedEventArgs = new TweetReceivedEventArgs(tweet);
            this.Raise(TweetCreatedByAnyone, tweetReceivedEventArgs);

            if (tweet.CreatedBy.Equals(_authenticatedUser))
            {
                this.Raise(TweetCreatedByMe, tweetReceivedEventArgs);
            }
            else
            {
                this.Raise(TweetCreatedByAnyoneButMe, tweetReceivedEventArgs);
            }

            if (_friendIds.Contains(tweet.CreatedBy.Id))
            {
                this.Raise(TweetCreatedByFriend, tweetReceivedEventArgs);
            }

            return true;
        }

        // Message
        private bool TryGetMessage(string jsonMessage)
        {
            var messageObject = _jObjectWrapper.GetJobjectFromJson(jsonMessage);
            JToken messageJToken;

            if (messageObject.TryGetValue("direct_message", out messageJToken))
            {
                var message = _messageFactory.GenerateMessageFromJson(messageJToken.ToString());
                if (message == null)
                {
                    return false;
                }

                var messageEventArgs = new MessageEventArgs(message);
                if (message.SenderId == _authenticatedUser.Id)
                {
                    this.Raise(MessageSent, messageEventArgs);
                }

                if (message.RecipientId == _authenticatedUser.Id)
                {
                    this.Raise(MessageReceived, messageEventArgs);
                }

                return true;
            }

            return false;
        }

        // Follow
        private void TryRaiseUserFollowedEvent(JObject userFollowedEvent)
        {
            var source = GetSourceUser(userFollowedEvent);
            var target = GetTargetUser(userFollowedEvent);

            if (source.Equals(_authenticatedUser))
            {
                if (!_friendIds.Contains(target.Id))
                {
                    _friendIds.Add(target.Id);
                }

                this.Raise(FollowedUser, new UserFollowedEventArgs(target));
            }
            else
            {
                this.Raise(FollowedByUser, new UserFollowedEventArgs(source));
            }
        }

        private void TryRaiseUserUnFollowedEvent(JObject userFollowedEvent)
        {
            var target = GetTargetUser(userFollowedEvent);

            if (_friendIds.Contains(target.Id))
            {
                _friendIds.Remove(target.Id);
            }

            this.Raise(UnFollowedUser, new UserFollowedEventArgs(target));
        }

        // Friends
        private bool TryGetFriends(string friendIdsJson)
        {
            JObject friendIdsObject = _jObjectWrapper.GetJobjectFromJson(friendIdsJson);
            JToken friendIdsToken;

            if (friendIdsObject.TryGetValue("friends", out friendIdsToken))
            {
                var friendIds = friendIdsToken.Values<long>();
                _friendIds = new HashSet<long>(friendIds);

                this.Raise(StreamIsReady);
                this.Raise(FriendIdsReceived, new GenericEventArgs<IEnumerable<long>>(friendIds));

                return true;
            }

            return false;
        }

        // Favourite
        private void TryRaiseFavouriteEvent(JObject favouriteEvent)
        {
            var tweet = GetTweet(favouriteEvent);
            var source = GetSourceUser(favouriteEvent);

            var tweetFavouritedEventArgs = new TweetFavouritedEventArgs(tweet, source);

            this.Raise(TweetFavouritedByAnyone, tweetFavouritedEventArgs);

            if (source.Equals(_authenticatedUser))
            {
                this.Raise(TweetFavouritedByMe, tweetFavouritedEventArgs);
            }
            else
            {
                this.Raise(TweetFavouritedByAnyoneButMe, tweetFavouritedEventArgs);
            }
        }

        private void TryRaiseUnFavouriteEvent(JObject unFavouritedEvent)
        {
            var tweet = GetTweet(unFavouritedEvent);
            var source = GetSourceUser(unFavouritedEvent);

            var tweetFavouritedEventArgs = new TweetFavouritedEventArgs(tweet, source);

            this.Raise(TweetUnFavouritedByAnyone, tweetFavouritedEventArgs);

            if (source.Equals(_authenticatedUser))
            {
                this.Raise(TweetUnFavouritedByMe, tweetFavouritedEventArgs);
            }
            else
            {
                this.Raise(TweetUnFavouritedByAnyoneButMe, tweetFavouritedEventArgs);
            }
        }

        // List Created
        private void TryRaiseListCreatedEvent(JObject listCreatedEvent)
        {
            var list = GetList(listCreatedEvent);
            this.Raise(ListCreated, new ListEventArgs(list));
        }

        private void TryRaiseListUpdatedEvent(JObject listUpdatedEvent)
        {
            var list = GetList(listUpdatedEvent);
            this.Raise(ListUpdated, new ListEventArgs(list));
        }

        private void TryRaiseListDestroyedEvent(JObject listDestroyedEvent)
        {
            var list = GetList(listDestroyedEvent);
            this.Raise(ListDestroyed, new ListEventArgs(list));
        }

        private void TryRaiseListMemberAddedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_authenticatedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(AuthenticatedUserAddedMemberToList, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(AuthenticatedUserAddedToListBy, listEventArgs);
            }
        }

        private void TryRaiseListMemberRemovedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_authenticatedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(AuthenticatedUserRemovedMemberFromList, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(AuthenticatedUserRemovedFromListBy, listEventArgs);
            }
        }

        private void TryRaiseListMemberSubscribedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_authenticatedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(AuthenticatedUserSubscribedToListCreatedBy, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(UserSubscribedToListCreatedByMe, listEventArgs);
            }
        }

        private void TryRaiseListMemberUnsubscribedEvent(JObject listMemberAddedEvent)
        {
            var list = GetList(listMemberAddedEvent);
            var source = GetSourceUser(listMemberAddedEvent);
            var target = GetTargetUser(listMemberAddedEvent);

            if (source.Equals(_authenticatedUser))
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, target);
                this.Raise(AuthenticatedUserUnsubscribedToListCreatedBy, listEventArgs);
            }
            else
            {
                var listEventArgs = new ListUserUpdatedEventArgs(list, source);
                this.Raise(UserUnsubscribedToListCreatedByMe, listEventArgs);
            }
        }

        // Block
        private void TryRaiseUserBlockedEvent(JObject userBlockedEvent)
        {
            var target = GetTargetUser(userBlockedEvent);
            this.Raise(BlockedUser, new UserBlockedEventArgs(target));
        }

        private void TryRaiseUserUnBlockedEvent(JObject userUnBlockedEvent)
        {
            var target = GetTargetUser(userUnBlockedEvent);
            this.Raise(UnBlockedUser, new UserBlockedEventArgs(target));
        }

        // Tweet
        private void TryRaiseQuotedTweetEvent(JObject userQuotedTweet)
        {
            var tweetDTO = _jObjectWrapper.ToObject<ITweetDTO>(userQuotedTweet["target_object"]);
            var tweet = _tweetFactory.GenerateTweetFromDTO(tweetDTO);

            TryRaiseTweetEvent(tweet);
        }

        // User Update
        private void TryRaiseUserUpdatedEvent(JObject userUpdatedEvent)
        {
            var source = GetSourceUser(userUpdatedEvent);
            var newAuthenticatedUser = _userFactory.GenerateAuthenticatedUserFromDTO(source.UserDTO);

            this.Raise(AuthenticatedUserProfileUpdated, new AuthenticatedUserUpdatedEventArgs(newAuthenticatedUser));
        }

        // Warnings
        private bool TryGetWarning(string warningJson)
        {
            var jsonObjectWarning = _jObjectWrapper.GetJobjectFromJson(warningJson);
            JToken jsonWarning;

            if (jsonObjectWarning.TryGetValue("warning", out jsonWarning))
            {
                return TryRaiseTooMuchFollowerWarning(jsonWarning);
            }

            return false;
        }

        private bool TryRaiseTooMuchFollowerWarning(JToken jsonWarning)
        {
            if (jsonWarning["user_id"] != null)
            {
                var warningMessage = _jsonObjectConverter.DeserializeObject<WarningMessageTooManyFollowers>(jsonWarning.ToString());
                this.Raise(WarningTooManyFollowersDetected, new WarningTooManyFollowersEventArgs(warningMessage));
                return true;
            }

            return false;
        }

        // Credentials
        private void TryRaiseAccessRevokedEvent(JObject accessRevoked)
        {
            var accessRevokedInfo = _jsonObjectConverter.DeserializeObject<AccessRevokedInfo>(accessRevoked["target_object"].ToString());
            var source = GetSourceUser(accessRevoked);
            var target = GetTargetUser(accessRevoked);

            this.Raise(AccessRevoked, new AccessRevokedEventArgs(accessRevokedInfo, source, target));
        }

        #endregion

        #region Get Json Info

        private IUser GetSourceUser(JObject eventInfo)
        {
            var jsonSource = eventInfo["source"].ToString();
            return _userFactory.GenerateUserFromJson(jsonSource);
        }

        private IUser GetTargetUser(JObject eventInfo)
        {
            var jsonTarget = eventInfo["target"].ToString();
            return _userFactory.GenerateUserFromJson(jsonTarget);
        }

        private ITwitterList GetList(JObject listEvent)
        {
            var jsonList = listEvent["target_object"].ToString();
            return _twitterListFactory.CreateListFromJson(jsonList);
        }

        private ITweet GetTweet(JObject tweetEvent)
        {
            var jsonTweet = tweetEvent["target_object"].ToString();
            return _tweetFactory.GenerateTweetFromJson(jsonTweet);
        }

        #endregion
    }
}