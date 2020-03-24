namespace Tweetinvi.Parameters
{
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
        }

        /// <inheritdoc/>
        public int PageSize { get; set; }
        /// <inheritdoc/>
        public long? SinceId { get; set; }
        /// <inheritdoc/>
        public long? MaxId { get; set; }
    }
}