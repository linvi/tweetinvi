using System.Text.RegularExpressions;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public interface ICredentialsCreator
    {
        ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret);
        IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials);
    }

    public class CredentialsCreator : ICredentialsCreator
    {
        private readonly ICredentialsFactory _credentialsFactory;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly IFactory<ITemporaryCredentials> _applicationCredentialsUnityFactory;

        public CredentialsCreator(
            ICredentialsFactory credentialsFactory,
            IExceptionHandler exceptionHandler,
            ITwitterRequestHandler twitterRequestHandler,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IFactory<ITemporaryCredentials> applicationCredentialsUnityFactory)
        {
            _credentialsFactory = credentialsFactory;
            _exceptionHandler = exceptionHandler;
            _twitterRequestHandler = twitterRequestHandler;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _applicationCredentialsUnityFactory = applicationCredentialsUnityFactory;
        }

        // Step 0 - Generate Temporary Credentials
        public ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret)
        {
            var consumerKeyParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerKey", consumerKey);
            var consumerSecretParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerSecret", consumerSecret);

            return _applicationCredentialsUnityFactory.Create(consumerKeyParameterOverride, consumerSecretParameterOverride);
        }

        // Step 2 - Generate User Credentials
        public IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials)
        {
            try
            {
                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true, true, false);
                var response = _twitterRequestHandler.ExecuteQueryWithTemporaryCredentials(Resources.OAuthRequestAccessToken, HttpMethod.POST, temporaryCredentials, new[] {callbackParameter});

                if (response == null)
                {
                    return null;
                }

                Match responseInformation = Regex.Match(response, Resources.OAuthTokenAccessRegex);
                if (responseInformation.Groups["oauth_token"] == null || responseInformation.Groups["oauth_token_secret"] == null)
                {
                    return null;
                }

                var credentials = _credentialsFactory.CreateOAuthCredentials(
                    responseInformation.Groups["oauth_token"].Value,
                    responseInformation.Groups["oauth_token_secret"].Value,
                    temporaryCredentials.ConsumerKey,
                    temporaryCredentials.ConsumerSecret);

                return credentials;
            }
            catch (TwitterException ex)
            {
                if (_exceptionHandler.LogExceptions)
                {
                    _exceptionHandler.AddTwitterException(ex);
                }

                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }
            }

            return null;
        }
    }
}