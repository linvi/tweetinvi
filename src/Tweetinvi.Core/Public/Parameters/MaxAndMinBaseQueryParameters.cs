namespace Tweetinvi.Parameters
{
    public enum ContinueMinMaxCursor
    {
        /// <summary>
        /// The iterator will be marked as completed when no more items are returned.
        /// This implies additional requests.
        /// </summary>
        UntilNoItemsReturned,

        /// <summary>
        /// The iterator will be marked as completed when the number of items returned is lower than requested.
        /// </summary>
        UntilPageSizeIsDifferentFromRequested,
    }

    public interface IMinMaxQueryParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Minimum id that can be returned by the query (start from)
        /// </summary>
        long? SinceId { get; set; }

        /// <summary>
        /// Maximum id that can be returned by the query (ends at)
        /// </summary>
        long? MaxId { get; set; }

        /// <summary>
        /// Defines when the cursor should stop
        /// </summary>
        ContinueMinMaxCursor ContinueMinMaxCursor { get; set; }
    }

    public class MinMaxQueryParameters : CustomRequestParameters, IMinMaxQueryParameters
    {
        protected MinMaxQueryParameters()
        {
        }

        protected MinMaxQueryParameters(IMinMaxQueryParameters source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            PageSize = source.PageSize;
            SinceId = source.SinceId;
            MaxId = source.MaxId;
            ContinueMinMaxCursor = source.ContinueMinMaxCursor;
        }

        /// <inheritdoc/>
        public int PageSize { get; set; }
        /// <inheritdoc/>
        public long? SinceId { get; set; }
        /// <inheritdoc/>
        public long? MaxId { get; set; }
        /// <inheritdoc/>
        public ContinueMinMaxCursor ContinueMinMaxCursor { get; set; }
    }
}