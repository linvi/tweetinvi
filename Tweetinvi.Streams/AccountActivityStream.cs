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

namespace Tweetinvi.Streams
{
    public class AccountActivityStream : IAccountActivityStream
    {
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetFactory _tweetFactory;
        private readonly ITwitterCredentials _credentials;
        private readonly Dictionary<string, Action<JToken>> _events;

        public AccountActivityStream(
            IJObjectStaticWrapper jObjectWrapper,
            IJsonObjectConverter jsonObjectConverter,
            ITweetFactory tweetFactory)
        {
            _jObjectWrapper = jObjectWrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetFactory = tweetFactory;
            _events = new Dictionary<string, Action<JToken>>();

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _events.Add("tweet_create_events", TryRaiseTweetCreatedEvents);
        }

        public long UserId { get; private set; }
        public EventHandler<TweetReceivedEventArgs> TweetCreated { get; set; }
        public EventHandler<TweetFavouritedEventArgs> TweetFavourited { get; set; }


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
    }
}
