using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public interface IStreamsV2Client
    {
        ISampleStreamV2 CreateSampleStream();
        IFilteredStreamV2 CreateFilteredStream();

        Task<FilteredStreamRulesV2ResponseDTO> GetRulesForFilteredStreamV2Async();
        Task<FilteredStreamRulesV2ResponseDTO> GetRulesForFilteredStreamV2Async(params string[] ruleIds);
        Task<FilteredStreamRulesV2ResponseDTO> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2ResponseDTO> AddRulesToFilteredStreamAsync(params FilteredStreamRuleConfig[] rulesToAdd);
        Task<FilteredStreamRulesV2ResponseDTO> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2ResponseDTO> DeleteRulesFromFilteredStreamAsync(params string[] ruleIdsToDelete);
        Task<FilteredStreamRulesV2ResponseDTO> DeleteRulesFromFilteredStreamAsync(params FilteredStreamRuleDTO[] rulesToDelete);
        Task<FilteredStreamRulesV2ResponseDTO> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters);

        Task<FilteredStreamRulesV2ResponseDTO> TestFilteredStreamRulesV2Async(params FilteredStreamRuleConfig[] rulesToAdd);
        Task<FilteredStreamRulesV2ResponseDTO> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters);

    }
}