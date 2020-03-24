using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup
    /// </summary>
    public interface IGetTweetsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The identifiers of the tweets you want to retrieve
        /// </summary>
        ITweetIdentifier[] Tweets { get; set; }

        /// <summary>
        /// Tweet's author object will not be populated when set to true
        /// </summary>
        bool? TrimUser { get; set; }

        /// <summary>
        /// Tweet's entities will not be included if set to false
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// Tweet's alt text attached to media will be included when set to true
        /// </summary>
        bool? IncludeExtAltText { get; set; }

        /// <summary>
        /// Tweet's card uri will be included when set to true
        /// </summary>
        bool? IncludeCardUri { get; set; }
    }

    /// <inheritdoc/>
    public class GetTweetsParameters : CustomRequestParameters, IGetTweetsParameters
    {
        public GetTweetsParameters()
        {
        }

        public GetTweetsParameters(long[] tweetIds)
        {
            Tweets = tweetIds?.Select(x => new TweetIdentifier(x) as ITweetIdentifier).ToArray();
        }

        public GetTweetsParameters(ITweetIdentifier[] tweetIdentifiers)
        {
            Tweets = tweetIdentifiers;
        }

        /// <inheritdoc/>
        public ITweetIdentifier[] Tweets { get; set; }
        /// <inheritdoc/>
        public bool? TrimUser { get; set; }
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc/>
        public bool? IncludeExtAltText { get; set; }
        /// <inheritdoc/>
        public bool? IncludeCardUri { get; set; }
    }
}