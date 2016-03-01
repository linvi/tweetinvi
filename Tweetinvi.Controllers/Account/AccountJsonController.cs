using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountJsonController
    {
        string  GetAuthenticatedUserSettingsJson();
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

        public string GetAuthenticatedUserSettingsJson()
        {
            string query = _accountQueryGenerator.GetAuthenticatedUserAccountSettingsQuery();
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }
    }
}