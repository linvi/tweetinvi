using System;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAuthClientRequiredParametersValidator : IAuthClientParametersValidator
    {
    }

    public class AuthClientRequiredParametersValidator : IAuthClientRequiredParametersValidator
    {
        public void ValidateCreateBearerToken(ITwitterRequest request)
        {
            var credentials = request.Query.TwitterCredentials;

            if (string.IsNullOrEmpty(credentials?.ConsumerKey))
            {
                throw new ArgumentException("Cannot be null or empty", $"{nameof(credentials)}{nameof(credentials.ConsumerKey)}");
            }

            if (string.IsNullOrEmpty(credentials?.ConsumerSecret))
            {
                throw new ArgumentException("Cannot be null or empty", $"{nameof(credentials)}{nameof(credentials.ConsumerSecret)}");
            }
        }

        public void Validate(IRequestAuthUrlParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.RequestId == string.Empty)
            {
                throw new ArgumentException("Cannot be empty", $"{nameof(parameters)}{nameof(parameters.RequestId)}");
            }
        }

        public void Validate(IRequestCredentialsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrEmpty(parameters.VerifierCode))
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.VerifierCode)}", "If you received a null verifier code, the authentication failed");
            }

            if (parameters.AuthToken == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthToken)}");
            }

            if (string.IsNullOrEmpty(parameters.AuthToken.ConsumerKey))
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthToken)}{nameof(parameters.AuthToken.ConsumerKey)}");
            }

            if (string.IsNullOrEmpty(parameters.AuthToken.ConsumerSecret))
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthToken)}{nameof(parameters.AuthToken.ConsumerSecret)}");
            }
        }
    }
}