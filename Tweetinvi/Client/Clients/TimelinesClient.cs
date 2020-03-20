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
        public Task<ITweet[]> GetHomeTimeline()
        {
            return GetHomeTimeline(new GetHomeTimelineParameters());
        }

        public async Task<ITweet[]> GetHomeTimeline(IGetHomeTimelineParameters parameters)
        {
            var iterator = GetHomeTimelineIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
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

        public Task<ITweet[]> GetUserTimeline(long userId)
        {
            return GetUserTimeline(new GetUserTimelineParameters(userId));
        }

        public Task<ITweet[]> GetUserTimeline(string username)
        {
            return GetUserTimeline(new GetUserTimelineParameters(username));
        }

        public Task<ITweet[]> GetUserTimeline(IUserIdentifier user)
        {
            return GetUserTimeline(new GetUserTimelineParameters(user));
        }

        public async Task<ITweet[]> GetUserTimeline(IGetUserTimelineParameters parameters)
        {
            var iterator = GetUserTimelineIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
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

        public Task<ITweet[]> GetMentionsTimeline()
        {
            return GetMentionsTimeline(new GetMentionsTimelineParameters());
        }

        public async Task<ITweet[]> GetMentionsTimeline(IGetMentionsTimelineParameters parameters)
        {
            var iterator = GetMentionsTimelineIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
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

        public Task<ITweet[]> GetRetweetsOfMeTimeline()
        {
            return GetRetweetsOfMeTimeline(new GetRetweetsOfMeTimelineParameters());
        }

        public async Task<ITweet[]> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters)
        {
            var iterator = GetRetweetsOfMeTimelineIterator(parameters);
            var firstResults = await iterator.MoveToNextPage().ConfigureAwait(false);
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