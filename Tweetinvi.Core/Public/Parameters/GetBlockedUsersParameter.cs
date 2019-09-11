using Tweetinvi.Core.Public.Parameters;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// Parameters to get a user's blocked users
    /// </summary>
    public interface IGetBlockedUsersParameters : ICursorQueryParameters
    {
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// When set to true, statuses will not be included in the returned user objects.
        /// </summary>
        bool? SkipStatus { get; set; }
    }

    public class GetBlockedUsersParameters : CursorQueryParameters, IGetBlockedUsersParameters
    {
        public GetBlockedUsersParameters()
        {
            PageSize = 5000;
        }

        public GetBlockedUsersParameters(IGetBlockedUsersParameters source) : base(source)
        {
            IncludeEntities = source.IncludeEntities;
            SkipStatus = source.SkipStatus;
        }

        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
    }
}
