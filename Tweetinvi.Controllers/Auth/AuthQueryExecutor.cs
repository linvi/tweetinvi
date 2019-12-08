using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryExecutor
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request);
        Task<ITwitterResult> RequestAuthUrl(RequestAuthUrlInternalParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RequestCredentials(IRequestCredentialsParameters parameters, ITwitterRequest request);
    }

    public class AuthQueryExecutor : IAuthQueryExecutor
    {
        private readonly IAuthQueryGenerator _queryGenerator;
        private readonly IOAuthWebRequestGeneratorFactory _oAuthWebRequestGeneratorFactory;
        private readonly ITwitterAccessor _twitterAccessor;

        public AuthQueryExecutor(
            IAuthQueryGenerator queryGenerator,
            IOAuthWebRequestGeneratorFactory oAuthWebRequestGeneratorFactory,
            ITwitterAccessor twitterAccessor)
        {
            _queryGenerator = queryGenerator;
            _oAuthWebRequestGeneratorFactory = oAuthWebRequestGeneratorFactory;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request)
        {
            var oAuthQueryGenerator = _oAuthWebRequestGeneratorFactory.Create(request);
            request.Query.Url = _queryGenerator.GetCreateBearerTokenQuery();
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new BearerHttpHandler(oAuthQueryGenerator);
            return _twitterAccessor.ExecuteRequest<CreateTokenResponseDTO>(request);
        }

        public Task<ITwitterResult> RequestAuthUrl(RequestAuthUrlInternalParameters parameters, ITwitterRequest request)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_callback", parameters.CallbackUrl, true, true, false);

            request.Query.Url = _queryGenerator.GetRequestAuthUrlQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new AuthHttpHandler(callbackParameter, parameters.AuthRequestToken, oAuthWebRequestGenerator);

            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> RequestCredentials(IRequestCredentialsParameters parameters, ITwitterRequest request)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", parameters.VerifierCode, true, true, false);

            request.Query.Url = _queryGenerator.GetRequestCredentialsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.TwitterCredentials = new TwitterCredentials(parameters.AuthRequestToken.ConsumerKey, parameters.AuthRequestToken.ConsumerSecret);
            request.TwitterClientHandler = new AuthHttpHandler(callbackParameter, parameters.AuthRequestToken, oAuthWebRequestGenerator);

            return _twitterAccessor.ExecuteRequest(request);
        }
    }
}