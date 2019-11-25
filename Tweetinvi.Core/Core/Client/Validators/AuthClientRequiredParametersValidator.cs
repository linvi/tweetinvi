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

        public void Validate(IStartAuthProcessParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.AuthorizationId == string.Empty)
            {
                throw new ArgumentException("Cannot be empty", $"{nameof(parameters)}{nameof(parameters.AuthorizationId)}");
            }
        }
    }
}