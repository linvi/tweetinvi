using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Help
{
    public interface IHelpJsonController
    {
        Task<string> GetCredentialsRateLimits();
        Task<string> GetTwitterPrivacyPolicy();
    }

    public class HelpJsonController : IHelpJsonController
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public HelpJsonController(
            IHelpQueryGenerator helpQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<string> GetCredentialsRateLimits()
        {
            string query = _helpQueryGenerator.GetCredentialsLimitsQuery();
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> GetTwitterPrivacyPolicy()
        {
            string query = _helpQueryGenerator.GetTwitterPrivacyPolicyQuery();
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}