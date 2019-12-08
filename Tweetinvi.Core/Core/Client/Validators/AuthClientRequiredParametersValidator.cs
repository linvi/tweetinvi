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

            if (parameters.AuthRequest == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthRequest)}");
            }

            if (string.IsNullOrEmpty(parameters.AuthRequest.ConsumerKey))
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthRequest)}{nameof(parameters.AuthRequest.ConsumerKey)}");
            }

            if (string.IsNullOrEmpty(parameters.AuthRequest.ConsumerSecret))
            {
                throw new ArgumentNullException($"{nameof(parameters)}{nameof(parameters.AuthRequest)}{nameof(parameters.AuthRequest.ConsumerSecret)}");
            }
        }
    }
}