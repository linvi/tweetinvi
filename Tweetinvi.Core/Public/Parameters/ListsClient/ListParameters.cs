using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <inheritdoc />
    public interface IListParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Identifier of a twitter list
        /// </summary>
        ITwitterListIdentifier List { get; set; }
    }

    /// <inheritdoc />
    public class ListParameters : CustomRequestParameters, IListParameters
    {
        public ListParameters(long? listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public ListParameters(ITwitterListIdentifier list)
        {
            List = list;
        }

        /// <inheritdoc />
        public ITwitterListIdentifier List { get; set; }
    }
}