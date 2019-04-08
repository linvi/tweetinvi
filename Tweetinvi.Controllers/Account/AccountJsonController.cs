using System.Threading.Tasks;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountJsonController
    {
        Task<string> GetAuthenticatedUserSettingsJson();
    }

    public class AccountJsonController : IAccountJsonController
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IAccountQueryGenerator _accountQueryGenerator;

        public AccountJsonController(
            ITwitterAccessor twitterAccessor,
            IAccountQueryGenerator accountQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _accountQueryGenerator = accountQueryGenerator;
        }

        public Task<string> GetAuthenticatedUserSettingsJson()
        {
            string query = _accountQueryGenerator.GetAuthenticatedUserAccountSettingsQuery();
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}