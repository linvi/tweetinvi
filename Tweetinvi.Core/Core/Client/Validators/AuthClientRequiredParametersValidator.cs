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
        public void Validate(ICreateBearerTokenParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var credentials = request.Query.TwitterCredentials;
            ThrowIfInvalidConsumerCredentials("client.Credentials", credentials);
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
                throw new ArgumentNullException($"{nameof(parameters)}.{nameof(parameters.VerifierCode)}", "If you received a null verifier code, the authentication failed");
            }

            if (parameters.AuthRequest == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}.{nameof(parameters.AuthRequest)}");
            }

            ThrowIfInvalidConsumerCredentials($"{nameof(parameters)}.{nameof(parameters.AuthRequest)}", parameters.AuthRequest);
        }

        public void Validate(IInvalidateAccessTokenParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var credentialsParameterName = "client.Credentials";
            var credentials = request.Query.TwitterCredentials;

            ThrowIfInvalidConsumerCredentials(credentialsParameterName, credentials);
            ThrowIfInvalidAccessCredentials(credentialsParameterName, credentials);
        }

        public void Validate(IInvalidateBearerTokenParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var credentialsParameterName = $"client.Credentials";
            var credentials = request.Query.TwitterCredentials;

            ThrowIfInvalidConsumerCredentials(credentialsParameterName, credentials);

            if (string.IsNullOrEmpty(credentials?.BearerToken))
            {
                throw new ArgumentException("Cannot be null or empty", $"{credentialsParameterName}.{nameof(credentials.BearerToken)}");
            }
        }

        public static void ThrowIfInvalidConsumerCredentials(string credentialsParameterName, IReadOnlyConsumerCredentialsWithoutBearer credentials)
        {
            if (string.IsNullOrEmpty(credentials?.ConsumerKey))
            {
                throw new ArgumentException("Cannot be null or empty", $"{credentialsParameterName}.{nameof(credentials.ConsumerKey)}");
            }

            if (string.IsNullOrEmpty(credentials.ConsumerSecret))
            {
                throw new ArgumentException("Cannot be null or empty", $"{credentialsParameterName}.{nameof(credentials.ConsumerSecret)}");
            }
        }

        public static void ThrowIfInvalidAccessCredentials(string credentialsParameterName, IReadOnlyTwitterCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials?.AccessToken))
            {
                throw new ArgumentException("Cannot be null or empty", $"{credentialsParameterName}.{nameof(credentials.AccessToken)}");
            }

            if (string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                throw new ArgumentException("Cannot be null or empty", $"{credentialsParameterName}.{nameof(credentials.AccessTokenSecret)}");
            }
        }
    }
}