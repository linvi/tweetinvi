using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Combines results and the cursor returned from Twitter into a single object.
    /// This is used to return results from async methods replacing "out", which can only be used synchronously.
    /// 
    /// Note that this is only for newer Twitter API endpoints returning a string cursor rather than
    /// requiring parameters such as since_id. An example of an endpoint working like this is getting DMs:
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events
    /// </summary>
    public interface IResultsWithCursor<T>
    {
        /// <summary>
        /// Query results.
        /// </summary>
        IEnumerable<T> Results { get; set; }

        /// <summary>
        /// Cursor to be used for fetching more results.
        /// If null, no more results are available.
        /// </summary>
        string Cursor { get; set; }
    }
}
