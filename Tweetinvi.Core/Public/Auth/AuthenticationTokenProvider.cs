using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Auth
{
    public interface IAuthenticationTokenProvider
    {
        /// <summary>
        /// Creates a unique identifier that will be used a RequestId/TokenId for an authentication process
        /// </summary>
        string GenerateAuthTokenId();

        /// <summary>
        /// The name of the parameter that will be added to the callback url to identify which request/token
        /// the callback is associated with
        /// </summary>
        string CallbackTokenIdParameterName();

        /// <summary>
        /// Logic to extract the token from a callback url
        /// </summary>
        string ExtractTokenIdFromCallbackUrl(string callbackUrl);

        /// <summary>
        /// Returns the authentication token information from its id
        /// </summary>
        Task<IAuthenticationRequestToken> GetAuthenticationTokenFromId(string requestId);

        /// <summary>
        /// Logic to add an authenticationToken to the store
        /// </summary>
        Task AddAuthenticationToken(IAuthenticationRequestToken authenticationRequestToken);

        /// <summary>
        /// Logic to remove an authenticationToken from the store
        /// </summary>
        Task RemoveAuthenticationToken(string requestId);
    }

    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class AuthenticationTokenProvider : IAuthenticationTokenProvider
    {
        private readonly ConcurrentDictionary<string, IAuthenticationRequestToken> _store;

        public AuthenticationTokenProvider()
        {
            _store = new ConcurrentDictionary<string, IAuthenticationRequestToken>();
        }

        public virtual string CallbackTokenIdParameterName()
        {
            return "tweetinvi_auth_request_id";
        }

        public virtual string ExtractTokenIdFromCallbackUrl(string callbackUrl)
        {
            var queryParameter = CallbackTokenIdParameterName();
            var tokenId = callbackUrl.GetURLParameter(queryParameter);

            if (tokenId == null)
            {
                throw new ArgumentException($"Could not extract the token id as '{queryParameter}' was not part of the url", nameof(callbackUrl));
            }

            return tokenId;
        }

        public virtual string GenerateAuthTokenId()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual Task<IAuthenticationRequestToken> GetAuthenticationTokenFromId(string requestId)
        {
            _store.TryGetValue(requestId, out var authenticationToken);
            return Task.FromResult(authenticationToken);
        }

        public virtual Task AddAuthenticationToken(IAuthenticationRequestToken authenticationRequestToken)
        {
            _store.AddOrUpdate(authenticationRequestToken.Id, s => authenticationRequestToken, (s, token) => authenticationRequestToken);
            return Task.CompletedTask;
        }

        public virtual Task RemoveAuthenticationToken(string requestId)
        {
            _store.TryRemove(requestId, out _);
            return Task.CompletedTask;
        }
    }
}