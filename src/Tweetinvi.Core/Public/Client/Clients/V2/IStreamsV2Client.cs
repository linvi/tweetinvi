using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public interface IStreamsV2Client
    {
        ISampleStreamV2 CreateSampleStream();
        IFilteredStreamV2 CreateFilteredStream();

        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async();
        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(params string[] ruleIds);
        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(params FilteredStreamRuleConfig[] rulesToAdd);
        Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(params string[] ruleIdsToDelete);
        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(params FilteredStreamRuleV2[] rulesToDelete);
        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(params FilteredStreamRuleConfig[] rulesToAdd);
        Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters);

    }
}