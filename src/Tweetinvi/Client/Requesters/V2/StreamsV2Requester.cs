using System.Threading.Tasks;
using Tweetinvi.Controllers.Streams;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface IStreamsV2Requester
    {
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters);
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters);
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters);
        Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters);
    }

    public class StreamsV2Requester : BaseRequester, IStreamsV2Requester
    {
        private readonly IStreamsV2Controller _streamsV2Controller;

        public StreamsV2Requester(
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents,
            IStreamsV2Controller streamsV2Controller) : base(client, twitterClientEvents)
        {
            _streamsV2Controller = streamsV2Controller;
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _streamsV2Controller.GetRulesForFilteredStreamV2Async(parameters, request));
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _streamsV2Controller.AddRulesToFilteredStreamAsync(parameters, request));
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _streamsV2Controller.DeleteRulesFromFilteredStreamAsync(parameters, request));
        }

        public Task<ITwitterResult<FilteredStreamRulesV2ResponseDTO>> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _streamsV2Controller.TestFilteredStreamRulesV2Async(parameters, request));
        }
    }
}