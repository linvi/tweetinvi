using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class FilteredStream : TrackedStream, IFilteredStream
    {
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ISingleAggregateExceptionThrower _singleAggregateExceptionThrower;
        // Const
        private const int MAXIMUM_TRACKED_LOCATIONS_AUTHORIZED = 25;
        private const int MAXIMUM_TRACKED_USER_ID_AUTHORIZED = 5000;

        // Filters
        public MatchOn MatchOn { get; set; }

        // Properties
        private readonly Dictionary<long?, Action<ITweet>> _followingUserIds;
        public Dictionary<long?, Action<ITweet>> FollowingUserIds
        {
            get { return _followingUserIds; }
        }

        private readonly Dictionary<ILocation, Action<ITweet>> _locations;
        public Dictionary<ILocation, Action<ITweet>> Locations
        {
            get { return _locations; }
        }

        // Constructor
        public FilteredStream(
            IStreamTrackManager<ITweet> streamTrackManager,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            ISynchronousInvoker synchronousInvoker,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory,
            ISingleAggregateExceptionThrower singleAggregateExceptionThrower)

            : base(
                streamTrackManager,
                jsonObjectConverter,
                jObjectStaticWrapper,
                streamResultGenerator,
                tweetFactory,
                synchronousInvoker,
                customRequestParameters,
                twitterQueryFactory,
                singleAggregateExceptionThrower)
        {
            _twitterQueryFactory = twitterQueryFactory;
            _singleAggregateExceptionThrower = singleAggregateExceptionThrower;
            _followingUserIds = new Dictionary<long?, Action<ITweet>>();
            _locations = new Dictionary<ILocation, Action<ITweet>>();

            MatchOn = MatchOn.Everything;
        }

        public void StartStreamMatchingAnyCondition()
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(() => StartStreamMatchingAnyConditionAsync());
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamMatchingAnyConditionAsync()
        {
            Func<ITwitterQuery> generateWebRequest = () =>
            {
                var queryBuilder = GenerateORFilterQuery();
                AddBaseParametersToQuery(queryBuilder);

                return _twitterQueryFactory.Create(queryBuilder.ToString(), HttpMethod.POST, Credentials);
            };

            Action<string> tweetReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var matchingTracksEvenArgs = GetMatchingTweetEventArgsAndRaiseMatchingElements(tweet);

                var matchingTracks = matchingTracksEvenArgs.MatchingTracks;
                var matchingLocations = matchingTracksEvenArgs.MatchingLocations;
                var matchingFollowers = matchingTracksEvenArgs.MatchingFollowers;

                RaiseTweetReceived(matchingTracksEvenArgs);

                if (matchingTracks.Length != 0 || matchingLocations.Length != 0 || matchingFollowers.Length != 0)
                {
                    RaiseMatchingTweetReceived(matchingTracksEvenArgs);
                }
                else
                {
                    RaiseNonMatchingTweetReceived(new TweetEventArgs(tweet));
                }
            };

            await _streamResultGenerator.StartStreamAsync(tweetReceived, generateWebRequest);
        }

        public void StartStreamMatchingAllConditions()
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(() => StartStreamMatchingAllConditionsAsync());
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamMatchingAllConditionsAsync()
        {
            Func<ITwitterQuery> generateTwitterQuery = () =>
            {
                var queryBuilder = GenerateANDFilterQuery();
                AddBaseParametersToQuery(queryBuilder);

                return _twitterQueryFactory.Create(queryBuilder.ToString(), HttpMethod.POST, Credentials);
            };

            Action<string> tweetReceived = json =>
            {
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var matchingTracksEvenArgs = GetMatchingTweetEventArgsAndRaiseMatchingElements(tweet);

                var matchingTracks = matchingTracksEvenArgs.MatchingTracks;
                var matchingLocations = matchingTracksEvenArgs.MatchingLocations;
                var matchingFollowers = matchingTracksEvenArgs.MatchingFollowers;

                RaiseTweetReceived(matchingTracksEvenArgs);

                if (DoestTheTweetMatchAllConditions(tweet, matchingTracks, matchingLocations, matchingFollowers))
                {
                    RaiseMatchingTweetReceived(matchingTracksEvenArgs);
                }
                else
                {
                    RaiseNonMatchingTweetReceived(new TweetEventArgs(tweet));
                }
            };

            await _streamResultGenerator.StartStreamAsync(tweetReceived, generateTwitterQuery);
        }

        private MatchedTweetReceivedEventArgs GetMatchingTweetEventArgsAndRaiseMatchingElements(ITweet tweet)
        {
            var matchingTracksEventArgs = new MatchedTweetReceivedEventArgs(tweet);

            var matchingTrackAndActions = new Dictionary<string, Action<ITweet>>();
            var matchingLocationAndActions = new Dictionary<ILocation, Action<ITweet>>();
            var matchingFollowersAndActions = new Dictionary<long, Action<ITweet>>();

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.TweetText))
            {
                var tracksMatchingTweetText = _streamTrackManager.GetMatchingTracksAndActions(tweet.Text);

                tracksMatchingTweetText.ForEach(x =>
                {
                    matchingTrackAndActions.TryAdd(x.Item1, x.Item2);
                });

                if (tracksMatchingTweetText.Count > 0)
                {
                    matchingTracksEventArgs.MatchOn |= MatchOn.TweetText;
                }
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.AllEntities) || 
                MatchOn.HasFlag(MatchOn.URLEntities))
            {
                var expandedURLs = tweet.Entities.Urls.Select(x => x.ExpandedURL);
                expandedURLs = expandedURLs.Union(tweet.Entities.Medias.Select(x => x.ExpandedURL));

                expandedURLs.ForEach(x =>
                {
                    var tracksMatchingExpandedURL = _streamTrackManager.GetMatchingTracksAndActions(x);
                    tracksMatchingExpandedURL.ForEach(t =>
                    {
                        matchingTrackAndActions.TryAdd(t.Item1, t.Item2);
                    });

                    if (tracksMatchingExpandedURL.Count > 0)
                    {
                        matchingTracksEventArgs.MatchOn |= MatchOn.URLEntities;
                    }
                });

                var displayedURLs = tweet.Entities.Urls.Select(x => x.DisplayedURL);
                displayedURLs = displayedURLs.Union(tweet.Entities.Medias.Select(x => x.DisplayURL));

                displayedURLs.ForEach(x =>
                {
                    var tracksMatchingDisplayedURL = _streamTrackManager.GetMatchingTracksAndActions(x);

                    tracksMatchingDisplayedURL.ForEach(t =>
                    {
                        matchingTrackAndActions.TryAdd(t.Item1, t.Item2);
                    });

                    if (tracksMatchingDisplayedURL.Count > 0)
                    {
                        matchingTracksEventArgs.MatchOn |= MatchOn.URLEntities;
                    }
                });
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.AllEntities) || 
                MatchOn.HasFlag(MatchOn.HashTagEntities))
            {
                var hashTags = tweet.Entities.Hashtags.Select(x => x.Text);

                hashTags.ForEach(x =>
                {
                    var tracksMatchingHashTag = _streamTrackManager.GetMatchingTracksAndActions(x);

                    tracksMatchingHashTag.ForEach(t =>
                    {
                        matchingTrackAndActions.TryAdd(t.Item1, t.Item2);
                    });

                    if (tracksMatchingHashTag.Count > 0)
                    {
                        matchingTracksEventArgs.MatchOn |= MatchOn.HashTagEntities;
                    }
                });
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.AllEntities) || 
                MatchOn.HasFlag(MatchOn.UserMentionEntities))
            {
                var mentionsScreenName = tweet.Entities.UserMentions.Select(x => x.ScreenName);

                mentionsScreenName.ForEach(x =>
                {
                    var tracksMatchingMentionScreenName = _streamTrackManager.GetMatchingTracksAndActions(x);

                    tracksMatchingMentionScreenName.ForEach(t =>
                    {
                        matchingTrackAndActions.TryAdd(t.Item1, t.Item2);
                    });

                    if (tracksMatchingMentionScreenName.Count > 0)
                    {
                        matchingTracksEventArgs.MatchOn |= MatchOn.UserMentionEntities;
                    }
                });
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.TweetLocation))
            {
                var matchedLocations = GetMatchedLocations(tweet).ToArray();

                matchedLocations.ForEach(x =>
                {
                    matchingLocationAndActions.TryAdd(x.Key, x.Value);
                });

                if (matchedLocations.Length > 0)
                {
                    matchingTracksEventArgs.MatchOn |= MatchOn.TweetLocation;
                }
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.Follower))
            {
                var userId = tweet.CreatedBy?.Id;
                Action<ITweet> actionToExecuteWhenMatchingFollower;

                if (userId != null && _followingUserIds.TryGetValue(userId, out actionToExecuteWhenMatchingFollower))
                {
                    matchingFollowersAndActions.TryAdd(userId.Value, actionToExecuteWhenMatchingFollower);
                    matchingTracksEventArgs.MatchOn |= MatchOn.Follower;
                }
            }

            if (MatchOn.HasFlag(MatchOn.Everything) ||
                MatchOn.HasFlag(MatchOn.FollowerInReplyTo))
            {
                var userId = tweet.InReplyToUserId;
                Action<ITweet> actionToExecuteWhenMatchingFollower;

                if (userId != null && _followingUserIds.TryGetValue(userId, out actionToExecuteWhenMatchingFollower))
                {
                    matchingFollowersAndActions.TryAdd(userId.Value, actionToExecuteWhenMatchingFollower);
                    matchingTracksEventArgs.MatchOn |= MatchOn.FollowerInReplyTo;
                }
            }

            var matchingTracks = matchingTrackAndActions.Select(x => x.Key).ToArray();
            var matchingLocations = matchingLocationAndActions.Select(x => x.Key).ToArray();
            var matchingFollowers = matchingFollowersAndActions.Select(x => x.Key).ToArray();

            matchingTracksEventArgs.MatchingTracks = matchingTracks;
            matchingTracksEventArgs.MatchingLocations = matchingLocations;
            matchingTracksEventArgs.MatchingFollowers = matchingFollowers;

            CallMultipleActions(tweet, matchingTrackAndActions.Select(x => x.Value));
            CallMultipleActions(tweet, matchingLocationAndActions.Select(x => x.Value));
            CallMultipleActions(tweet, matchingFollowersAndActions.Select(x => x.Value));

            return matchingTracksEventArgs;
        }

        private void CallMultipleActions<T>(T tweet, IEnumerable<Action<T>> actions)
        {
            if (actions != null)
            {
                actions.ForEach(action =>
                {
                    if (action != null)
                    {
                        action(tweet);
                    }
                });
            }
        }

        private bool DoestTheTweetMatchAllConditions(ITweet tweet, string[] matchingTracks, ILocation[] matchingLocations, long[] matchingFollowers)
        {
            if (tweet.CreatedBy.Id == TweetinviSettings.DEFAULT_ID)
            {
                return false;
            }

            bool followMatches = FollowingUserIds.IsEmpty() || matchingFollowers.Any();
            bool tracksMatches = Tracks.IsEmpty() || matchingTracks.Any();
            bool locationMatches = Locations.IsEmpty() || matchingLocations.Any();

            if (FollowingUserIds.Any())
            {
                return followMatches && tracksMatches && locationMatches;
            }

            if (Tracks.Any())
            {
                return tracksMatches && locationMatches;
            }

            if (Locations.Any())
            {
                return locationMatches;
            }

            return true;
        }

        #region Generate Query

        private StringBuilder GenerateORFilterQuery()
        {
            var queryBuilder = new StringBuilder(Resources.Stream_Filter);

            var followPostRequest = QueryGeneratorHelper.GenerateFilterFollowRequest(FollowingUserIds.Keys.ToList());
            var trackPostRequest = QueryGeneratorHelper.GenerateFilterTrackRequest(Tracks.Keys.ToList());
            var locationPostRequest = QueryGeneratorHelper.GenerateFilterLocationRequest(Locations.Keys.ToList());

            if (!string.IsNullOrEmpty(trackPostRequest))
            {
                queryBuilder.Append(trackPostRequest);
            }

            if (!string.IsNullOrEmpty(followPostRequest))
            {
                queryBuilder.Append(queryBuilder.Length == 0 ? followPostRequest : string.Format("&{0}", followPostRequest));
            }

            if (!string.IsNullOrEmpty(locationPostRequest))
            {
                queryBuilder.Append(queryBuilder.Length == 0 ? locationPostRequest : string.Format("&{0}", locationPostRequest));
            }

            return queryBuilder;
        }

        private StringBuilder GenerateANDFilterQuery()
        {
            var queryBuilder = new StringBuilder(Resources.Stream_Filter);

            var followPostRequest = QueryGeneratorHelper.GenerateFilterFollowRequest(FollowingUserIds.Keys.ToList());
            var trackPostRequest = QueryGeneratorHelper.GenerateFilterTrackRequest(Tracks.Keys.ToList());
            var locationPostRequest = QueryGeneratorHelper.GenerateFilterLocationRequest(Locations.Keys.ToList());

            if (!string.IsNullOrEmpty(followPostRequest))
            {
                queryBuilder.Append(followPostRequest);
            }
            else if (!string.IsNullOrEmpty(trackPostRequest))
            {
                queryBuilder.Append(trackPostRequest);
            }
            else if (!string.IsNullOrEmpty(locationPostRequest))
            {
                queryBuilder.Append(locationPostRequest);
            }

            return queryBuilder;
        }

        #endregion

        #region Follow

        public void AddFollow(long? userId, Action<ITweet> userPublishedTweet = null)
        {
            if (userId != null && _followingUserIds.Count < MAXIMUM_TRACKED_USER_ID_AUTHORIZED)
            {
                _followingUserIds.Add(userId, userPublishedTweet);
            }
        }

        public void AddFollow(IUser user, Action<ITweet> userPublishedTweet = null)
        {
            if (user != null && user.Id != TweetinviSettings.DEFAULT_ID)
            {
                AddFollow(user.Id, userPublishedTweet);
            }
        }

        public void RemoveFollow(long? userId)
        {
            if (userId != null)
            {
                _followingUserIds.Remove(userId);
            }
        }

        public void RemoveFollow(IUser user)
        {
            if (user != null)
            {
                RemoveFollow(user.Id);
            }
        }

        public bool ContainsFollow(long? userId)
        {
            if (userId != null)
            {
                return _followingUserIds.Keys.Contains(userId);
            }

            return false;
        }

        public bool ContainsFollow(IUser user)
        {
            if (user != null)
            {
                return ContainsFollow(user.Id);
            }

            return false;
        }

        public void ClearFollows()
        {
            _followingUserIds.Clear();
        }

        #endregion

        #region Location

        public ILocation AddLocation(ICoordinates coordinate1, ICoordinates coordinate2, Action<ITweet> locationDetected = null)
        {
            ILocation location = new Location(coordinate1, coordinate2);
            AddLocation(location, locationDetected);

            return location;
        }

        public void AddLocation(ILocation location, Action<ITweet> locationDetected = null)
        {
            if (!ContainsLocation(location) && _locations.Count < MAXIMUM_TRACKED_LOCATIONS_AUTHORIZED)
            {
                Locations.Add(location, locationDetected);
            }
        }

        public void RemoveLocation(ICoordinates coordinate1, ICoordinates coordinate2)
        {
            var location = Locations.Keys.FirstOrDefault(x => (x.Coordinate1 == coordinate1 && x.Coordinate2 == coordinate2) ||
                                                              (x.Coordinate1 == coordinate2 && x.Coordinate2 == coordinate1));

            if (location != null)
            {
                Locations.Remove(location);
            }
        }

        public void RemoveLocation(ILocation location)
        {
            RemoveLocation(location.Coordinate1, location.Coordinate2);
        }

        public bool ContainsLocation(ICoordinates coordinate1, ICoordinates coordinate2)
        {
            return Locations.Keys.Any(x => (x.Coordinate1 == coordinate1 && x.Coordinate2 == coordinate2) ||
                                           (x.Coordinate1 == coordinate2 && x.Coordinate2 == coordinate1));
        }

        public bool ContainsLocation(ILocation location)
        {
            return ContainsLocation(location.Coordinate1, location.Coordinate2);
        }

        public void ClearLocations()
        {
            Locations.Clear();
        }

        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(ITweet tweet)
        {
            var tweetCoordinates = tweet.Coordinates;
            if (tweetCoordinates != null)
            {
                return GetMatchedLocations(tweetCoordinates);
            }

            var place = tweet.Place;
            if (place != null)
            {
                var boundingBox = place.BoundingBox;
                if (boundingBox != null)
                {
                    var placeCoordinates = boundingBox.Coordinates;
                    return GetMatchedLocations(placeCoordinates);
                }
            }

            return new List<KeyValuePair<ILocation, Action<ITweet>>>();
        }

        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(IEnumerable<ICoordinates> coordinates)
        {
            var top = coordinates.Max(x => x.Longitude);
            var left = coordinates.Min(x => x.Latitude);

            var bottom = coordinates.Min(x => x.Longitude);
            var right = coordinates.Max(x => x.Latitude);

            var matchingLocations = new List<KeyValuePair<ILocation, Action<ITweet>>>();
            foreach (var locationAndAction in _locations)
            {
                var location = locationAndAction.Key;

                var filterTop = Math.Max(location.Coordinate1.Longitude, location.Coordinate2.Longitude);
                var filterLeft = Math.Min(location.Coordinate1.Latitude, location.Coordinate2.Latitude);

                var filterBottom = Math.Min(location.Coordinate1.Longitude, location.Coordinate2.Longitude);
                var filterRight = Math.Max(location.Coordinate1.Latitude, location.Coordinate2.Latitude);

                var isTweetOutsideOfLocationCoordinates = left > filterRight || right < filterLeft || top < filterBottom || bottom > filterTop;

                if (!isTweetOutsideOfLocationCoordinates)
                {
                    matchingLocations.Add(locationAndAction);
                }
            }

            return matchingLocations;
        }

        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(ICoordinates coordinates)
        {
            if (_locations.IsEmpty() || coordinates == null)
            {
                return new List<KeyValuePair<ILocation, Action<ITweet>>>();
            }

            return _locations.Where(x => Location.CoordinatesLocatedIn(coordinates, x.Key)).ToList();
        }

        #endregion
    }
}