namespace Tweetinvi.Core.Public.Parameters
{
    /// <summary>
    /// A query that can return multiple times based on a cursor value
    /// </summary>
    public interface ICursorQueryParameters
    {
        /// <summary>
        /// The cursor value to start the operation with
        /// </summary>
        int Cursor { get; set; }

        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        int MaximumNumberOfResults { get; set; }
    }

    public class CursorQueryParameters : ICursorQueryParameters
    {
        public CursorQueryParameters()
        {
            Cursor = -1;
            MaximumNumberOfResults = 20;
        }

        public int Cursor { get; set; }

        public int MaximumNumberOfResults { get; set; }
    }
}
