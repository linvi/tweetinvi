using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryExecutor
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request);
    }

    public class AuthQueryExecutor : IAuthQueryExecutor
    {
        private readonly IAuthQueryGenerator _queryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public AuthQueryExecutor(
            IAuthQueryGenerator queryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _queryGenerator = queryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request)
        {
            var query = _queryGenerator.GetCreateBearerTokenQuery();
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new BearerHttpHandler();
            return _twitterAccessor.ExecuteRequest<CreateTokenResponseDTO>(request);
        }
    }
}