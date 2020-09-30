using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Streams
{
    public interface IStreamsV2Controller
    {
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters, ITwitterRequest request);
    }

    public class StreamsV2Controller : IStreamsV2Controller
    {
        private readonly IStreamsV2QueryExecutor _streamsV2QueryExecutor;

        public StreamsV2Controller(IStreamsV2QueryExecutor streamsV2QueryExecutor)
        {
            _streamsV2QueryExecutor = streamsV2QueryExecutor;
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters, ITwitterRequest request)
        {
            return _streamsV2QueryExecutor.GetRulesForFilteredStreamV2Async(parameters, request);
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters, ITwitterRequest request)
        {
            return _streamsV2QueryExecutor.AddRulesToFilteredStreamAsync(parameters, request);
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters, ITwitterRequest request)
        {
            return _streamsV2QueryExecutor.DeleteRulesFromFilteredStreamAsync(parameters, request);
        }
    }
}