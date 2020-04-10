using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Auth
{
    public interface IAuthenticationRequestStore
    {
        /// <summary>
        /// Append an AuthenticationRequest identifier to a callback url
        /// </summary>
        string AppendAuthenticationRequestIdToCallbackUrl(string callbackUrl, string authenticationRequestId);

        /// <summary>
        /// Extract the AuthenticationRequest identifier from the received callback url
        /// </summary>
        string ExtractAuthenticationRequestIdFromCallbackUrl(string callbackUrl);

        /// <summary>
        /// Returns the AuthenticationRequest from its identifier
        /// </summary>
        Task<IAuthenticationRequest> GetAuthenticationRequestFromId(string authenticationRequestTokenId);

        /// <summary>
        /// Stores the AuthenticationRequest
        /// </summary>
        Task AddAuthenticationToken(string authenticationRequestId, IAuthenticationRequest authenticationRequest);

        /// <summary>
        /// Removes an AuthenticationRequest from the store
        /// </summary>
        Task RemoveAuthenticationToken(string authenticationRequestId);
    }

    public class LocalAuthenticationRequestStore : IAuthenticationRequestStore
    {
        private readonly ConcurrentDictionary<string, IAuthenticationRequest> _store;

        public LocalAuthenticationRequestStore()
        {
            _store = new ConcurrentDictionary<string, IAuthenticationRequest>();
        }

        private string CallbackTokenIdParameterName { get; } = "tweetinvi_auth_request_id";

        public virtual string ExtractAuthenticationRequestIdFromCallbackUrl(string callbackUrl)
        {
            var queryParameter = CallbackTokenIdParameterName;
            var tokenId = callbackUrl.GetURLParameter(queryParameter);

            if (tokenId == null)
            {
                throw new ArgumentException($"Could not extract the token id as '{queryParameter}' was not part of the url", nameof(callbackUrl));
            }

            return tokenId;
        }

        public virtual string AppendAuthenticationRequestIdToCallbackUrl(string callbackUrl, string authenticationRequestId)
        {
            return callbackUrl.AddParameterToQuery(CallbackTokenIdParameterName, authenticationRequestId);
        }

        public virtual Task<IAuthenticationRequest> GetAuthenticationRequestFromId(string authenticationRequestTokenId)
        {
            _store.TryGetValue(authenticationRequestTokenId, out var authenticationRequest);
            return Task.FromResult(authenticationRequest);
        }

        public virtual Task AddAuthenticationToken(string authenticationRequestId, IAuthenticationRequest authenticationRequest)
        {
            _store.AddOrUpdate(authenticationRequestId, s => authenticationRequest, (s, token) => authenticationRequest);
            return Task.CompletedTask;
        }

        public virtual Task RemoveAuthenticationToken(string authenticationRequestId)
        {
            _store.TryRemove(authenticationRequestId, out _);
            return Task.CompletedTask;
        }
    }
}