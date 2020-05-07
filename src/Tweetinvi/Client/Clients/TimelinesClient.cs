using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class TimelinesClient : ITimelinesClient
    {
        private readonly ITwitterClient _client;
        private readonly ITimelinesRequester _timelinesRequester;

        public TimelinesClient(ITwitterClient client)
        {
            _client = client;
            _timelinesRequester = _client.Raw.Timelines;
        }

        public ITimelineClientParametersValidator ParametersValidator => _client.ParametersValidator;
        public Task<ITweet[]> GetHomeTimelineAsync()
        {
            return GetHomeTimelineAsync(new GetHomeTimelineParameters());
        }

        public async Task<ITweet[]> GetHomeTimelineAsync(IGetHomeTimelineParameters parameters)
        {
            var iterator = GetHomeTimelineIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator()
        {
            return GetHomeTimelineIterator(new GetHomeTimelineParameters());
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters)
        {
            var pageIterator = _timelinesRequester.GetHomeTimelineIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject));
        }

        public Task<ITweet[]> GetUserTimelineAsync(long userId)
        {
            return GetUserTimelineAsync(new GetUserTimelineParameters(userId));
        }

        public Task<ITweet[]> GetUserTimelineAsync(string username)
        {
            return GetUserTimelineAsync(new GetUserTimelineParameters(username));
        }

        public Task<ITweet[]> GetUserTimelineAsync(IUserIdentifier user)
        {
            return GetUserTimelineAsync(new GetUserTimelineParameters(user));
        }

        public async Task<ITweet[]> GetUserTimelineAsync(IGetUserTimelineParameters parameters)
        {
            var iterator = GetUserTimelineIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator(long userId)
        {
            return GetUserTimelineIterator(new GetUserTimelineParameters(userId));
        }

        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator(string username)
        {
            return GetUserTimelineIterator(new GetUserTimelineParameters(username));
        }

        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator(IUserIdentifier user)
        {
            return GetUserTimelineIterator(new GetUserTimelineParameters(user));
        }

        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator(IGetUserTimelineParameters parameters)
        {
            var pageIterator = _timelinesRequester.GetUserTimelineIterator(parameters);

            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject));
        }

        public Task<ITweet[]> GetMentionsTimelineAsync()
        {
            return GetMentionsTimelineAsync(new GetMentionsTimelineParameters());
        }

        public async Task<ITweet[]> GetMentionsTimelineAsync(IGetMentionsTimelineParameters parameters)
        {
            var iterator = GetMentionsTimelineIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator()
        {
            return GetMentionsTimelineIterator(new GetMentionsTimelineParameters());
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters)
        {
            var pageIterator = _timelinesRequester.GetMentionsTimelineIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject));
        }

        public Task<ITweet[]> GetRetweetsOfMeTimelineAsync()
        {
            return GetRetweetsOfMeTimelineAsync(new GetRetweetsOfMeTimelineParameters());
        }

        public async Task<ITweet[]> GetRetweetsOfMeTimelineAsync(IGetRetweetsOfMeTimelineParameters parameters)
        {
            var iterator = GetRetweetsOfMeTimelineIterator(parameters);
            var firstResults = await iterator.NextPageAsync().ConfigureAwait(false);
            return firstResults?.ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator()
        {
            return GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters());
        }

        public ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters)
        {
            var pageIterator = _timelinesRequester.GetRetweetsOfMeTimelineIterator(parameters);

            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject));
        }
    }
}