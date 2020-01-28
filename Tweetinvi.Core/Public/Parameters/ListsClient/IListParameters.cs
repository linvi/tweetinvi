using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <inheritdoc />
    public interface IListParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Identifier of a twitter list
        /// </summary>
        ITwitterListIdentifier Id { get; set; }
    }

    /// <inheritdoc />
    public class ListParameters : CustomRequestParameters, IListParameters
    {
        public ListParameters(ITwitterListIdentifier twitterListIdentifier)
        {
            Id = twitterListIdentifier;
        }

        /// <inheritdoc />
        public ITwitterListIdentifier Id { get; set; }
    }
}