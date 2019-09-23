namespace Tweetinvi.Parameters
{
    public interface IMaxAndMinBaseQueryParameters : ICustomRequestParameters
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
    
    public class MaxAndMinBaseQueryParameters : CustomRequestParameters, IMaxAndMinBaseQueryParameters
    {
        public MaxAndMinBaseQueryParameters()
        {
        }

        public MaxAndMinBaseQueryParameters(IMaxAndMinBaseQueryParameters source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            PageSize = source.PageSize;
            SinceId = source.SinceId;
            MaxId = source.MaxId;
        }

        public int PageSize { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
    }
}