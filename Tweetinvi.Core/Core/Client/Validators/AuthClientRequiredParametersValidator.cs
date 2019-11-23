using System;
using Tweetinvi.Models;

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
    }
}