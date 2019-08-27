namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Output of a cursor based requests
    /// </summary>
    public class CursorOperationResult<TItem>
    {
        /// <summary>
        /// Items returned during for a specific cursor iteration
        /// </summary>
        public TItem[] Items { get; set; }

        /// <summary>
        /// Cursor to get the next set of items
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Cursor to get the previous set of items
        /// </summary>
        public string PreviousCursor { get; set; }
    }
}
