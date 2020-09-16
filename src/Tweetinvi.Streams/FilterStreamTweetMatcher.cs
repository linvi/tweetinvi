using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Tweetinvi.Streams
{
    public interface IFilterStreamTweetMatcher
    {
        MatchedTweetReceivedEventArgs GetMatchingTweetEventArgsAndRaiseMatchingElements(ITweet tweet, string json, MatchOn matchOn);
    }

    public class FilterStreamTweetMatcher : IFilterStreamTweetMatcher
    {
        private readonly IStreamTrackManager<ITweet> _streamTrackManager;
        private readonly Dictionary<ILocation, Action<ITweet>> _locations;
        private readonly Dictionary<long?, Action<ITweet>> _followingUserIds;

        public FilterStreamTweetMatcher(
            IStreamTrackManager<ITweet> streamTrackManager,
            Dictionary<ILocation, Action<ITweet>> locations,
            Dictionary<long?, Action<ITweet>> followingUserIds)
        {
            _streamTrackManager = streamTrackManager;
            _locations = locations;
            _followingUserIds = followingUserIds;
        }

        public MatchedTweetReceivedEventArgs GetMatchingTweetEventArgsAndRaiseMatchingElements(ITweet tweet, string json, MatchOn matchOn)
        {
            var result = new MatchedTweetReceivedEventArgs(tweet, json);

            var trackMatcherConfig = new FilteredStreamMatcherConfig<string>(matchOn);
            var locationMatcherConfig = new FilteredStreamMatcherConfig<ILocation>(matchOn);
            var followersMatcherConfig = new FilteredStreamMatcherConfig<long>(matchOn);

            UpdateMatchesBasedOnTweetText(tweet, trackMatcherConfig, result);
            UpdateMatchesBasedOnUrlEntities(tweet, trackMatcherConfig, result);
            UpdateMatchesBasedOnHashTagEntities(tweet, trackMatcherConfig, result);
            UpdateMatchesBasedOnUserMentions(tweet, trackMatcherConfig, result);
            UpdateMatchesBasedOnSymbols(tweet, trackMatcherConfig, result);
            UpdateMatchesBasedOnTweetLocation(tweet, locationMatcherConfig, result);
            UpdateMatchesBasedOnTweetCreator(tweet, followersMatcherConfig, result);
            UpdateMatchesBasedOnTweetInReplyToUser(tweet, followersMatcherConfig, result);

            result.MatchingTracks = trackMatcherConfig.TweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.MatchingLocations = locationMatcherConfig.TweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.MatchingFollowers = followersMatcherConfig.TweetMatchingTrackAndActions.Select(x => x.Key).ToArray();

            result.RetweetMatchingTracks = trackMatcherConfig.RetweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.RetweetMatchingLocations = locationMatcherConfig.RetweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.RetweetMatchingFollowers = followersMatcherConfig.RetweetMatchingTrackAndActions.Select(x => x.Key).ToArray();

            result.QuotedTweetMatchingTracks = trackMatcherConfig.QuotedTweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.QuotedTweetMatchingLocations = locationMatcherConfig.QuotedTweetMatchingTrackAndActions.Select(x => x.Key).ToArray();
            result.QuotedTweetMatchingFollowers = followersMatcherConfig.QuotedTweetMatchingTrackAndActions.Select(x => x.Key).ToArray();

            CallMultipleActions(tweet, trackMatcherConfig.GetAllMatchingTracks().Select(x => x.Value));
            CallMultipleActions(tweet, locationMatcherConfig.GetAllMatchingTracks().Select(x => x.Value));
            CallMultipleActions(tweet, followersMatcherConfig.GetAllMatchingTracks().Select(x => x.Value));

            return result;
        }

        // Update Event Args
        private void UpdateMatchesBasedOnTweetText(ITweet tweet, FilteredStreamMatcherConfig<string> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.TweetText))
            {
                GetMatchingTracksInTweetText(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.TweetText);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingTracksInTweetText(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.TweetText);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingTracksInTweetText(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.TweetText);
                }
            }
        }

        private void GetMatchingTracksInTweetText(ITweet tweet, Dictionary<string, Action<ITweet>> matchingTrackAndActions, Action onTrackFound)
        {
            var tracksMatchingTweetText = _streamTrackManager.GetMatchingTracksAndActions(tweet.FullText);
            tracksMatchingTweetText.ForEach(x => { matchingTrackAndActions.TryAdd(x.Item1, x.Item2); });
            if (tracksMatchingTweetText.Count > 0)
            {
                onTrackFound();
            }
        }

        private void UpdateMatchesBasedOnUrlEntities(ITweet tweet, FilteredStreamMatcherConfig<string> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.AllEntities) ||
                config.MatchOn.HasFlag(MatchOn.URLEntities))
            {
                GetMatchingTracksInTweetUrls(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.URLEntities);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingTracksInTweetUrls(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.URLEntities);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingTracksInTweetUrls(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.URLEntities);
                }
            }
        }

        private void GetMatchingTracksInTweetUrls(
            ITweet tweet,
            Dictionary<string, Action<ITweet>> matchingTrackAndActions,
            Action onTrackFound)
        {
            var expandedUrls = tweet.Entities.Urls.Select(x => x.ExpandedURL);
            expandedUrls = expandedUrls.Union(tweet.Entities.Medias.Select(x => x.ExpandedURL));
            expandedUrls.ForEach(x =>
            {
                var tracksMatchingExpandedURL = _streamTrackManager.GetMatchingTracksAndActions(x);
                tracksMatchingExpandedURL.ForEach(t => { matchingTrackAndActions.TryAdd(t.Item1, t.Item2); });
                if (tracksMatchingExpandedURL.Count > 0)
                {
                    onTrackFound();
                }
            });

            var displayedUrls = tweet.Entities.Urls.Select(x => x.DisplayedURL);
            displayedUrls = displayedUrls.Union(tweet.Entities.Medias.Select(x => x.DisplayURL));
            displayedUrls.ForEach(x =>
            {
                var tracksMatchingDisplayedURL = _streamTrackManager.GetMatchingTracksAndActions(x);
                tracksMatchingDisplayedURL.ForEach(t => { matchingTrackAndActions.TryAdd(t.Item1, t.Item2); });
                if (tracksMatchingDisplayedURL.Count > 0)
                {
                    onTrackFound();
                }
            });
        }

        private void UpdateMatchesBasedOnHashTagEntities(ITweet tweet, FilteredStreamMatcherConfig<string> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.AllEntities) ||
                config.MatchOn.HasFlag(MatchOn.HashTagEntities))
            {
                GetMatchingTracksInHashTags(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.HashTagEntities);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingTracksInHashTags(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.HashTagEntities);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingTracksInHashTags(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.HashTagEntities);
                }
            }
        }

        private void GetMatchingTracksInHashTags(ITweet tweet, Dictionary<string, Action<ITweet>> matchingTrackAndActions, Action onTrackFound)
        {
            var hashTags = tweet.Entities.Hashtags.Select(x => x.Text);

            hashTags.ForEach(hashtag =>
            {
                var tracksMatchingHashTag = _streamTrackManager.GetMatchingTracksAndActions($"#{hashtag.ToLowerInvariant()}");
                tracksMatchingHashTag.ForEach(t => { matchingTrackAndActions.TryAdd(t.Item1, t.Item2); });
                if (tracksMatchingHashTag.Count > 0)
                {
                    onTrackFound();
                }
            });
        }

        private void UpdateMatchesBasedOnUserMentions(ITweet tweet, FilteredStreamMatcherConfig<string> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.AllEntities) ||
                config.MatchOn.HasFlag(MatchOn.UserMentionEntities))
            {
                GetMatchingTracksInUserMentions(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.UserMentionEntities);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingTracksInUserMentions(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.UserMentionEntities);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingTracksInUserMentions(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.UserMentionEntities);
                }
            }
        }

        private void GetMatchingTracksInUserMentions(ITweet tweet, Dictionary<string, Action<ITweet>> matchingTrackAndActions, Action onTrackFound)
        {
            var mentionsScreenName = tweet.Entities.UserMentions.Select(x => x.ScreenName);
            mentionsScreenName.ForEach(username =>
            {
                var tracksMatchingMentionScreenName = _streamTrackManager.GetMatchingTracksAndActions($"@{username.ToLowerInvariant()}");
                tracksMatchingMentionScreenName.ForEach(t => { matchingTrackAndActions.TryAdd(t.Item1, t.Item2); });
                if (tracksMatchingMentionScreenName.Count > 0)
                {
                    onTrackFound();
                }
            });
        }

        private void UpdateMatchesBasedOnSymbols(ITweet tweet, FilteredStreamMatcherConfig<string> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.AllEntities) ||
                config.MatchOn.HasFlag(MatchOn.SymbolEntities))
            {
                GetMatchingTracksInSymbols(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.SymbolEntities);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingTracksInSymbols(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.SymbolEntities);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingTracksInSymbols(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.SymbolEntities);
                }
            }
        }

        private void GetMatchingTracksInSymbols(ITweet tweet, Dictionary<string, Action<ITweet>> matchingTrackAndActions, Action onTrackFound)
        {
            var symbols = tweet.Entities.Symbols.Select(x => x.Text);
            symbols.ForEach(symbol =>
            {
                var tracksMatchingSymbol = _streamTrackManager.GetMatchingTracksAndActions($"${symbol.ToLowerInvariant()}");
                tracksMatchingSymbol.ForEach(t => { matchingTrackAndActions.TryAdd(t.Item1, t.Item2); });
                if (tracksMatchingSymbol.Count > 0)
                {
                    onTrackFound();
                }
            });
        }

        private void UpdateMatchesBasedOnTweetLocation(ITweet tweet, FilteredStreamMatcherConfig<ILocation> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.TweetLocation))
            {
                GetMatchingLocations(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.TweetLocation);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingLocations(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.TweetLocation);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingLocations(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.TweetLocation);
                }
            }
        }

        private void GetMatchingLocations(ITweet tweet, Dictionary<ILocation, Action<ITweet>> matchingLocationAndActions, Action onTrackFound)
        {
            var matchedLocations = GetMatchedLocations(tweet).ToArray();
            matchedLocations.ForEach(x => { matchingLocationAndActions.TryAdd(x.Key, x.Value); });
            if (matchedLocations.Length > 0)
            {
                onTrackFound();
            }
        }

        private void UpdateMatchesBasedOnTweetCreator(ITweet tweet, FilteredStreamMatcherConfig<long> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.Follower))
            {
                GetMatchingFollowersBasedOnTweetCreator(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.Follower);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingFollowersBasedOnTweetCreator(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.Follower);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingFollowersBasedOnTweetCreator(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.Follower);
                }
            }
        }

        private void GetMatchingFollowersBasedOnTweetCreator(ITweet tweet, Dictionary<long, Action<ITweet>> matchingFollowersAndActions, Action onTrackFound)
        {
            var userId = tweet.CreatedBy?.Id;
            Action<ITweet> actionToExecuteWhenMatchingFollower;

            if (userId != null && _followingUserIds.TryGetValue(userId, out actionToExecuteWhenMatchingFollower))
            {
                matchingFollowersAndActions.TryAdd(userId.Value, actionToExecuteWhenMatchingFollower);
                onTrackFound();
            }
        }

        private void UpdateMatchesBasedOnTweetInReplyToUser(ITweet tweet, FilteredStreamMatcherConfig<long> config, MatchedTweetReceivedEventArgs result)
        {
            if (config.MatchOn.HasFlag(MatchOn.Everything) ||
                config.MatchOn.HasFlag(MatchOn.FollowerInReplyTo))
            {
                GetMatchingFollowersBasedOnTweetReply(tweet, config.TweetMatchingTrackAndActions, () => result.MatchOn |= MatchOn.FollowerInReplyTo);

                if (tweet.RetweetedTweet != null)
                {
                    GetMatchingFollowersBasedOnTweetReply(tweet.RetweetedTweet, config.RetweetMatchingTrackAndActions, () => result.RetweetMatchOn |= MatchOn.FollowerInReplyTo);
                }

                if (tweet.QuotedTweet != null)
                {
                    GetMatchingFollowersBasedOnTweetReply(tweet.QuotedTweet, config.QuotedTweetMatchingTrackAndActions, () => result.QuotedTweetMatchOn |= MatchOn.FollowerInReplyTo);
                }
            }
        }

        private void GetMatchingFollowersBasedOnTweetReply(ITweet tweet, Dictionary<long, Action<ITweet>> matchingFollowersAndActions, Action onFollowersFound)
        {
            var userId = tweet.InReplyToUserId;
            Action<ITweet> actionToExecuteWhenMatchingFollower;

            if (userId != null && _followingUserIds.TryGetValue(userId, out actionToExecuteWhenMatchingFollower))
            {
                matchingFollowersAndActions.TryAdd(userId.Value, actionToExecuteWhenMatchingFollower);
                onFollowersFound();
            }
        }

        // Matched Locations
        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(ITweet tweet)
        {
            var tweetCoordinates = tweet.Coordinates;
            if (tweetCoordinates != null)
            {
                return GetMatchedLocations(tweetCoordinates);
            }

            var place = tweet.Place;
            var boundingBox = place?.BoundingBox;

            if (boundingBox != null)
            {
                var placeCoordinates = boundingBox.Coordinates;
                return GetMatchedLocations(placeCoordinates.ToArray());
            }

            return new List<KeyValuePair<ILocation, Action<ITweet>>>();
        }

        private IEnumerable<KeyValuePair<ILocation, Action<ITweet>>> GetMatchedLocations(ICoordinates[] coordinates)
        {
            var top = coordinates.Max(x => x.Latitude);
            var left = coordinates.Min(x => x.Longitude);

            var bottom = coordinates.Min(x => x.Latitude);
            var right = coordinates.Max(x => x.Longitude);

            var matchingLocations = new List<KeyValuePair<ILocation, Action<ITweet>>>();
            foreach (var locationAndAction in _locations)
            {
                var location = locationAndAction.Key;

                var filterTop = Math.Max(location.Coordinate1.Latitude, location.Coordinate2.Latitude);
                var filterLeft = Math.Min(location.Coordinate1.Longitude, location.Coordinate2.Longitude);

                var filterBottom = Math.Min(location.Coordinate1.Latitude, location.Coordinate2.Latitude);
                var filterRight = Math.Max(location.Coordinate1.Longitude, location.Coordinate2.Longitude);

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


        // Invoke callback actions
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
    }
}