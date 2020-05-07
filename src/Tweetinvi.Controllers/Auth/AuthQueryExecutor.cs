using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryExecutor
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RequestAuthUrlAsync(RequestAuthUrlInternalParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RequestCredentialsAsync(IRequestCredentialsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<InvalidateTokenResponse>> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<InvalidateTokenResponse>> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters, ITwitterRequest request)
        {
            var oAuthQueryGenerator = _oAuthWebRequestGeneratorFactory.Create(request);
            request.Query.Url = _queryGenerator.GetCreateBearerTokenQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new BearerHttpHandler(oAuthQueryGenerator);
            return _twitterAccessor.ExecuteRequestAsync<CreateTokenResponseDTO>(request);
        }

        public Task<ITwitterResult> RequestAuthUrlAsync(RequestAuthUrlInternalParameters parameters, ITwitterRequest request)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_callback", parameters.CallbackUrl, true, true, false);

            request.Query.Url = _queryGenerator.GetRequestAuthUrlQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new AuthHttpHandler(callbackParameter, parameters.AuthRequest, oAuthWebRequestGenerator);
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult> RequestCredentialsAsync(IRequestCredentialsParameters parameters, ITwitterRequest request)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", parameters.VerifierCode, true, true, false);

            request.Query.Url = _queryGenerator.GetRequestCredentialsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.TwitterCredentials = new TwitterCredentials(parameters.AuthRequest.ConsumerKey, parameters.AuthRequest.ConsumerSecret);
            request.TwitterClientHandler = new AuthHttpHandler(callbackParameter, parameters.AuthRequest, oAuthWebRequestGenerator);
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters, ITwitterRequest request)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();

            request.Query.Url = _queryGenerator.GetInvalidateBearerTokenQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            request.TwitterClientHandler = new InvalidateTokenHttpHandler(oAuthWebRequestGenerator);
            return _twitterAccessor.ExecuteRequestAsync<InvalidateTokenResponse>(request);
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _queryGenerator.GetInvalidateAccessTokenQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<InvalidateTokenResponse>(request);
        }
    }
}