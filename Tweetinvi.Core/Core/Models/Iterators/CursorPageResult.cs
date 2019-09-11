using System.Collections;
using System.Collections.Generic;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Output of a cursor based requests
    /// </summary>
    public class CursorPageResult<TItem> : PageResult<TItem>
    {
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
