using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Object iterating over cursored stored items in Twitter
    /// </summary>
    /// <typeparam name="TItem">Type of items returned by the cursor requests</typeparam>
    public interface ICursorResult<TItem>
    {
        /// <summary>
        /// Cursor to retrieve next data
        /// </summary>
        string NextCursor { get; }
        
        /// <summary>
        /// Cursor to retrieve previous data
        /// </summary>
        string PreviousCursor { get; }
        
        /// <summary>
        /// Whether all the data have been returned
        /// </summary>
        bool Completed { get; }
        
        /// <summary>
        /// List of aggregated items retrieved over the different cursor requests
        /// </summary>
        List<TItem> Items { get; }
        
        /// <summary>
        /// Items returned by the last cursor request
        /// </summary>
        TItem[] Current { get; }

        /// <summary>
        /// Move to the next cursor and update the items
        /// </summary>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<CursorOperationResult<TItem>> MoveNext();
        
        /// <summary>
        /// Move to the next cursor and update the items
        /// </summary>
        /// <param name="nextCursor">Cursor from where to get items</param>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<CursorOperationResult<TItem>> MoveNext(string nextCursor);
    }

    public class CursorResult<TItem, TDTO> : ICursorResult<TItem> where TDTO : IBaseCursorQueryDTO<TItem>
    {
        private readonly ITwitterCursorResult<TItem, TDTO> _twitterCursorResult;

        public CursorResult(ITwitterCursorResult<TItem, TDTO> twitterCursorResult)
        {
            if (twitterCursorResult == null)
            {
                throw new ArgumentNullException(nameof(twitterCursorResult));
            }
            
            _twitterCursorResult = twitterCursorResult;
        }

        public string PreviousCursor => _twitterCursorResult.PreviousCursor;
        public string NextCursor => _twitterCursorResult.NextCursor;
        public List<TItem> Items => _twitterCursorResult.Items.ToList();
        public TItem[] Current => _twitterCursorResult.Current;
        public bool Completed => _twitterCursorResult.Completed;

        public async Task<CursorOperationResult<TItem>> MoveNext()
        {
            var nextTwitterResults = await _twitterCursorResult.MoveNext();
            return CreateCursorOperationResult(nextTwitterResults);
        }
        
        public async Task<CursorOperationResult<TItem>> MoveNext(string nextCursor)
        {
            var nextTwitterResults = await _twitterCursorResult.MoveNext(nextCursor);
            return CreateCursorOperationResult(nextTwitterResults);
        }

        private static CursorOperationResult<TItem> CreateCursorOperationResult(ITwitterResult<TDTO> twitterResults)
        {
            var cursorResult = twitterResults.DataTransferObject;

            var items = cursorResult.Results.ToArray();

            return new CursorOperationResult<TItem>
            {
                Items = items,
                NextCursor = cursorResult.NextCursorStr,
                PreviousCursor = cursorResult.PreviousCursorStr
            };
        }
    }
}
