namespace Tweetinvi.Core.Iterators
{
    public interface ICursorPageResult<TItem, TCursor> : IPageResult<TItem>
    {
        /// <summary>
        /// Cursor to get the next set of items
        /// </summary>
        TCursor NextCursor { get; set; }
    }
    
    /// <summary>
    /// Output of a cursor based requests
    /// </summary>
    public class CursorPageResult<TItem, TCursor> : PageResult<TItem>, ICursorPageResult<TItem, TCursor>
    {
        /// <summary>
        /// Cursor to get the next set of items
        /// </summary>
        public TCursor NextCursor { get; set; }
    }
}
