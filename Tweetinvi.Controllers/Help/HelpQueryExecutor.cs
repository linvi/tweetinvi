using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpQueryExecutor
    {
        Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request);
    }

    public class HelpQueryExecutor : IHelpQueryExecutor
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public HelpQueryExecutor(IHelpQueryGenerator helpQueryGenerator, ITwitterAccessor twitterAccessor)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetRateLimitsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ICredentialsRateLimits>(request);
        }

        public Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _helpQueryGenerator.GetTwitterConfigurationQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterConfiguration>(request);
        }
    }
}