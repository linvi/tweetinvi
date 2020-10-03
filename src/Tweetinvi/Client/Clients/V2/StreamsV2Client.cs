using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public class StreamsV2Client : IStreamsV2Client
    {
        private readonly IFactory<ISampleStreamV2> _sampleStreamFactory;
        private readonly IFactory<IFilteredStreamV2> _filteredStreamFactory;
        private readonly IStreamsV2Requester _streamsV2Requester;

        public StreamsV2Client(
            IFactory<ISampleStreamV2> sampleStreamFactory,
            IFactory<IFilteredStreamV2> filteredStreamFactory,
            IStreamsV2Requester streamsV2Requester)
        {
            _sampleStreamFactory = sampleStreamFactory;
            _filteredStreamFactory = filteredStreamFactory;
            _streamsV2Requester = streamsV2Requester;
        }

        public ISampleStreamV2 CreateSampleStream()
        {
            return _sampleStreamFactory.Create();
        }

        public IFilteredStreamV2 CreateFilteredStream()
        {
            return _filteredStreamFactory.Create();
        }

        public Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async()
        {
            return GetRulesForFilteredStreamV2Async(new GetRulesForFilteredStreamV2Parameters());
        }

        public Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(params string[] ruleIds)
        {
            return GetRulesForFilteredStreamV2Async(new GetRulesForFilteredStreamV2Parameters(ruleIds));
        }

        public async Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters)
        {
            var twitterResult = await _streamsV2Requester.GetRulesForFilteredStreamV2Async(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(FilteredStreamRuleConfig[] rulesToAdd)
        {
            return AddRulesToFilteredStreamAsync(new AddRulesToFilteredStreamV2Parameters(rulesToAdd));
        }

        public async Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            var twitterResult = await _streamsV2Requester.AddRulesToFilteredStreamAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(string[] ruleIdsToDelete)
        {
            return DeleteRulesFromFilteredStreamAsync(new DeleteRulesFromFilteredStreamV2Parameters(ruleIdsToDelete));
        }

        public Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(params FilteredStreamRuleV2[] rulesToDelete)
        {
            var ruleIds = rulesToDelete.Select(x => x.Id).ToArray();
            return DeleteRulesFromFilteredStreamAsync(new DeleteRulesFromFilteredStreamV2Parameters(ruleIds));
        }

        public async Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters)
        {
            var twitterResult = await _streamsV2Requester.DeleteRulesFromFilteredStreamAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(params FilteredStreamRuleConfig[] rulesToAdd)
        {
            return TestFilteredStreamRulesV2Async(new AddRulesToFilteredStreamV2Parameters(rulesToAdd));
        }

        public async Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            var twitterResult = await _streamsV2Requester.TestFilteredStreamRulesV2Async(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }
    }
}