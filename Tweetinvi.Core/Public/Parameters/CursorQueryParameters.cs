using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Public.Parameters
{
    /// <summary>
    /// A query that can return multiple times based on a cursor value
    /// </summary>
    public interface ICursorQueryParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The cursor value to start the operation with
        /// </summary>
        string Cursor { get; set; }

        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        int MaximumNumberOfResults { get; set; }
    }

    public class CursorQueryParameters : CustomRequestParameters, ICursorQueryParameters
    {
        public CursorQueryParameters()
        {
            Cursor = null;
            MaximumNumberOfResults = 20;
        }

        public CursorQueryParameters(ICursorQueryParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                return;
            }
            
            Cursor = parameters.Cursor;
            MaximumNumberOfResults = parameters.MaximumNumberOfResults;
        }

        public string Cursor { get; set; }

        public int MaximumNumberOfResults { get; set; }
    }
}
