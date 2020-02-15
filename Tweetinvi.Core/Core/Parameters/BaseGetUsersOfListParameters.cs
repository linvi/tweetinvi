using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public interface IBaseGetUsersOfListParameters : IListParameters, ICursorQueryParameters
    {
        /// <summary>
        /// Users will include their entities when set to true
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// When set to true statuses will not be included in the returned user objects.
        /// </summary>
        bool? SkipStatus { get; set; }
    }

    public abstract class BaseGetUsersOfListParameters : CursorQueryParameters, IBaseGetUsersOfListParameters
    {
        protected BaseGetUsersOfListParameters(ITwitterListIdentifier list)
        {
            List = list;
        }

        protected BaseGetUsersOfListParameters(IBaseGetUsersOfListParameters parameters) : base(parameters)
        {
            List = parameters?.List;
            IncludeEntities = parameters?.IncludeEntities;
            SkipStatus = parameters?.SkipStatus;
        }

        /// <inheritdoc />
        public ITwitterListIdentifier List { get; set; }
        /// <inheritdoc />
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc />
        public bool? SkipStatus { get; set; }
    }
}