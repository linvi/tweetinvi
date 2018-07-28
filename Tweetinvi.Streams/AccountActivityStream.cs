using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streams.Model;

namespace Tweetinvi.Streams
{
    public class AccountActivityStream : IAccountActivityStream
    {
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterCredentials _credentials;
        private readonly Dictionary<string, Action<JToken>> _events;

        public AccountActivityStream(
            IJObjectStaticWrapper jObjectWrapper,
            IJsonObjectConverter jsonObjectConverter,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _jObjectWrapper = jObjectWrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _events = new Dictionary<string, Action<JToken>>();

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _events.Add("tweet_create_events", TryRaiseTweetCreatedEvents);
            _events.Add("favorite_events", TryRaiseTweetFavouritedEvents);
            _events.Add("follow_events", TryRaiseFollowedEvents);
        }

        public long UserId { get; set; }

        public EventHandler<TweetReceivedEventArgs> TweetCreated { get; set; }
        public EventHandler<TweetFavouritedEventArgs> TweetFavourited { get; set; }
        public EventHandler<UserFollowedEventArgs> UserFollowed { get; set; }


        public void WebhookMessageReceived(IWebhookMessage message)
        {
            var json = message.Json;
            var jsonObjectEvent = _jObjectWrapper.GetJobjectFromJson(json);

            var keys = jsonObjectEvent.Children().Where(x => x.Path != "for_user_id");
            var key = keys.SingleOrDefault();

            if (key == null)
            {
                return;
            }

            var eventName = key.Path;
            if (_events.ContainsKey(eventName))
            {
                _events[eventName].Invoke(jsonObjectEvent[eventName]);
            }
        }

        private void TryRaiseTweetCreatedEvents(JToken tweetCreatedEvent)
        {
            var jsonTweets = tweetCreatedEvent.ToString();
            var tweetDTOs = _jsonObjectConverter.DeserializeObject<ITweetDTO[]>(jsonTweets);

            tweetDTOs.ForEach(tweetDTO =>
            {
                var tweet = _tweetFactory.GenerateTweetFromDTO(tweetDTO);
                this.Raise(TweetCreated, new TweetReceivedEventArgs(tweet, "TODO"));
            });
        }

        private void TryRaiseTweetFavouritedEvents(JToken favouriteTweetEvent)
        {
            var jsonTweets = favouriteTweetEvent.ToString();
            var favouriteEventDTOs = _jsonObjectConverter.DeserializeObject<AccountActivityFavouriteEventDTO[]>(jsonTweets);

            favouriteEventDTOs.ForEach(favouriteEventDTO =>
            {
                var tweet = _tweetFactory.GenerateTweetFromDTO(favouriteEventDTO.FavouritedTweet);
                var user = _userFactory.GenerateUserFromDTO(favouriteEventDTO.User);
                this.Raise(TweetFavourited, new TweetFavouritedEventArgs(tweet, "TODO", user));
            });
        }

        private void TryRaiseFollowedEvents(JToken followEvent)
        {
            var jsonTweets = followEvent.ToString();
            var followedEventDTOs = _jsonObjectConverter.DeserializeObject<AccountActivityUserFollowedEventDTO[]>(jsonTweets);

            followedEventDTOs.ForEach(followedEventDTO =>
            {
                var source = followedEventDTO.Source;
                var target = followedEventDTO.Target;
                var followedUserDTO = source.Id == UserId ? target : source;
                var followedUser = _userFactory.GenerateUserFromDTO(followedUserDTO);
                this.Raise(UserFollowed, new UserFollowedEventArgs(followedUser));
            });
        }
    }
}
