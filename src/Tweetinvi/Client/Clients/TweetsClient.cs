using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class TweetsClient : ITweetsClient
    {
        private readonly ITwitterClient _client;
        private readonly ITweetsRequester _tweetsRequester;

        public TweetsClient(ITwitterClient client)
        {
            _client = client;
            _tweetsRequester = client.Raw.Tweets;
        }

        public ITweetsClientParametersValidator ParametersValidator => _client.ParametersValidator;

        // Tweets

        public Task<ITweet> GetTweetAsync(long tweetId)
        {
            return GetTweetAsync(new GetTweetParameters(tweetId));
        }

        public async Task<ITweet> GetTweetAsync(IGetTweetParameters parameters)
        {
            var twitterResult = await _tweetsRequester.GetTweetAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTweet(twitterResult?.Model);
        }

        public Task<ITweet[]> GetTweetsAsync(long[] tweetIds)
        {
            return GetTweetsAsync(new GetTweetsParameters(tweetIds));
        }

        public Task<ITweet[]> GetTweetsAsync(ITweetIdentifier[] tweets)
        {
            return GetTweetsAsync(new GetTweetsParameters(tweets));
        }

        public async Task<ITweet[]> GetTweetsAsync(IGetTweetsParameters parameters)
        {
            if (parameters.Tweets == null || parameters.Tweets.Length == 0)
            {
                return new ITweet[0];
            }

            var requestResult = await _tweetsRequester.GetTweetsAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTweets(requestResult?.Model);
        }

        // Tweets - Publish

        public Task<ITweet> PublishTweetAsync(string text)
        {
            return PublishTweetAsync(new PublishTweetParameters(text));
        }

        public async Task<ITweet> PublishTweetAsync(IPublishTweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishTweetAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTweet(requestResult?.Model);
        }

        // Tweets - Destroy

        public Task DestroyTweetAsync(long tweetId)
        {
            return DestroyTweetAsync(new DestroyTweetParameters(tweetId));
        }

        public Task DestroyTweetAsync(ITweetIdentifier tweet)
        {
            return DestroyTweetAsync(new DestroyTweetParameters(tweet));
        }

        public Task DestroyTweetAsync(ITweet tweet)
        {
            return DestroyTweetAsync(tweet.TweetDTO);
        }

        public async Task DestroyTweetAsync(ITweetDTO tweet)
        {
            await DestroyTweetAsync(new DestroyTweetParameters(tweet)).ConfigureAwait(false);
        }

        public async Task DestroyTweetAsync(IDestroyTweetParameters parameters)
        {
            await _tweetsRequester.DestroyTweetAsync(parameters).ConfigureAwait(false);
        }

        // Retweets

        public Task<ITweet[]> GetRetweetsAsync(long tweetId)
        {
            return GetRetweetsAsync(new GetRetweetsParameters(tweetId));
        }

        public Task<ITweet[]> GetRetweetsAsync(ITweetIdentifier tweet)
        {
            return GetRetweetsAsync(new GetRetweetsParameters(tweet));
        }

        public async Task<ITweet[]> GetRetweetsAsync(IGetRetweetsParameters parameters)
        {
            var requestResult = await _tweetsRequester.GetRetweetsAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTweets(requestResult?.Model);
        }

        public Task<ITweet> PublishRetweetAsync(long tweetId)
        {
            return PublishRetweetAsync(new PublishRetweetParameters(tweetId));
        }

        public Task<ITweet> PublishRetweetAsync(ITweetIdentifier tweet)
        {
            return PublishRetweetAsync(new PublishRetweetParameters(tweet));
        }

        public async Task<ITweet> PublishRetweetAsync(IPublishRetweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishRetweetAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTweet(requestResult?.Model);
        }

        public Task DestroyRetweetAsync(long retweetId)
        {
            return DestroyRetweetAsync(new DestroyRetweetParameters(retweetId));
        }

        public Task DestroyRetweetAsync(ITweetIdentifier retweet)
        {
            return DestroyRetweetAsync(new DestroyRetweetParameters(retweet));
        }

        public async Task DestroyRetweetAsync(IDestroyRetweetParameters parameters)
        {
            await _tweetsRequester.DestroyRetweetAsync(parameters).ConfigureAwait(false);
        }

        public Task<long[]> GetRetweeterIdsAsync(long tweetId)
        {
            return GetRetweeterIdsAsync(new GetRetweeterIdsParameters(tweetId));
        }

        public Task<long[]> GetRetweeterIdsAsync(ITweetIdentifier tweet)
        {
            return GetRetweeterIdsAsync(new GetRetweeterIdsParameters(tweet));
        }

        public async Task<long[]> GetRetweeterIdsAsync(IGetRetweeterIdsParameters parameters)
        {
            var iterator = GetRetweeterIdsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(long tweetId)
        {
            return GetRetweeterIdsIterator(new GetRetweeterIdsParameters(tweetId));
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(ITweetIdentifier tweet)
        {
            return GetRetweeterIdsIterator(new GetRetweeterIdsParameters(tweet));
        }

        public ITwitterIterator<long> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters)
        {
            var twitterResultIterator = _tweetsRequester.GetRetweeterIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterResultIterator, dto => dto.Model.Ids);
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(long userId)
        {
            return GetUserFavoriteTweetsAsync(new GetUserFavoriteTweetsParameters(userId));
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(string username)
        {
            return GetUserFavoriteTweetsAsync(new GetUserFavoriteTweetsParameters(username));
        }

        public Task<ITweet[]> GetUserFavoriteTweetsAsync(IUserIdentifier user)
        {
            return GetUserFavoriteTweetsAsync(new GetUserFavoriteTweetsParameters(user));
        }

        public async Task<ITweet[]> GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters parameters)
        {
            var iterator = GetUserFavoriteTweetsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        #region Favorite Tweets

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(long userId)
        {
            return GetUserFavoriteTweetsIterator(new GetUserFavoriteTweetsParameters(userId));
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(string username)
        {
            return GetUserFavoriteTweetsIterator(new GetUserFavoriteTweetsParameters(username));
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IUserIdentifier user)
        {
            return GetUserFavoriteTweetsIterator(new GetUserFavoriteTweetsParameters(user));
        }

        public ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters)
        {
            var favoriteTweetsIterator = _tweetsRequester.GetUserFavoriteTweetsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(favoriteTweetsIterator,
                twitterResult =>
                {
                    return twitterResult.Model.Select(x => _client.Factories.CreateTweet(x)).ToArray();
                });
        }

        public Task FavoriteTweetAsync(long tweetId)
        {
            return FavoriteTweetAsync(new FavoriteTweetParameters(tweetId));
        }

        public Task FavoriteTweetAsync(ITweetIdentifier tweet)
        {
            return FavoriteTweetAsync(new FavoriteTweetParameters(tweet));
        }

        public Task FavoriteTweetAsync(ITweet tweet)
        {
            return FavoriteTweetAsync(tweet.TweetDTO);
        }

        public async Task FavoriteTweetAsync(ITweetDTO tweet)
        {
            try
            {
                await FavoriteTweetAsync(new FavoriteTweetParameters(tweet)).ConfigureAwait(false);
                tweet.Favorited = true;
            }
            catch (TwitterException ex)
            {
                var tweetWasAlreadyFavorited = ex.TwitterExceptionInfos != null && ex.TwitterExceptionInfos.Any() && ex.TwitterExceptionInfos.First().Code == 139;
                if (tweetWasAlreadyFavorited)
                {
                    tweet.Favorited = true;
                    return;
                }

                throw;
            }
        }

        public async Task FavoriteTweetAsync(IFavoriteTweetParameters parameters)
        {
            await _tweetsRequester.FavoriteTweetAsync(parameters).ConfigureAwait(false);
        }

        public Task UnfavoriteTweetAsync(long tweetId)
        {
            return UnfavoriteTweetAsync(new UnfavoriteTweetParameters(tweetId));
        }

        public Task UnfavoriteTweetAsync(ITweetIdentifier tweet)
        {
            return UnfavoriteTweetAsync(new UnfavoriteTweetParameters(tweet));
        }

        public Task UnfavoriteTweetAsync(ITweet tweet)
        {
            return UnfavoriteTweetAsync(tweet.TweetDTO);
        }

        public async Task UnfavoriteTweetAsync(ITweetDTO tweet)
        {
            await UnfavoriteTweetAsync(new UnfavoriteTweetParameters(tweet)).ConfigureAwait(false);
            tweet.Favorited = false;
        }

        public async Task UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters)
        {
            await _tweetsRequester.UnfavoriteTweetAsync(parameters).ConfigureAwait(false);
        }

        public Task<IOEmbedTweet> GetOEmbedTweetAsync(ITweetIdentifier tweet)
        {
            return GetOEmbedTweetAsync(new GetOEmbedTweetParameters(tweet));
        }

        public Task<IOEmbedTweet> GetOEmbedTweetAsync(long tweetId)
        {
            return GetOEmbedTweetAsync(new GetOEmbedTweetParameters(tweetId));
        }

        public async Task<IOEmbedTweet> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters)
        {
            var twitterResult = await _tweetsRequester.GetOEmbedTweetAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateOEmbedTweet(twitterResult?.Model);
        }

        #endregion
    }
}
