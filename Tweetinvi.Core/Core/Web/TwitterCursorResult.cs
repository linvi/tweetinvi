using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Object iterating over cursored stored items in Twitter
    /// </summary>
    /// <typeparam name="TItem">Type of items returned by the cursor requests</typeparam>
    /// <typeparam name="TDTO">DataTransferObject returned by the cursor requests</typeparam>
    public interface ITwitterCursorResult<TItem, TDTO> where TDTO : IBaseCursorQueryDTO<TItem>
    {
        /// <summary>
        /// List of TwitterResults performed over the different cursor requests
        /// </summary>
        List<ITwitterResult<TDTO>> TwitterResults { get; }
        
        /// <summary>
        /// List of aggregated items retrieved over the different cursor requests
        /// </summary>
        List<TItem> Items { get; }
        
        /// <summary>
        /// Items returned by the last cursor request
        /// </summary>
        TItem[] Current { get;  }
        
        /// <summary>
        /// Cursor to retrieve next data
        /// </summary>
        string NextCursor { get; }
        
        /// <summary>
        /// Cursor to retrieve previous data
        /// </summary>
        string PreviousCursor { get;  }
        
        /// <summary>
        /// Whether all the data have been returned
        /// </summary>
        bool Completed { get; }
        
        /// <summary>
        /// Move to the next cursor and update the items
        /// </summary>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<ITwitterResult<TDTO>> MoveNext();
        
        /// <summary>
        /// Move to the defined cursor and update the items
        /// </summary>
        /// <param name="nextCursor">Cursor from where to get items</param>
        /// <returns>Items at the cursor</returns>
        Task<ITwitterResult<TDTO>> MoveNext(string nextCursor);
    }
    
    public class TwitterCursorResult<TItem, TDTO> : ITwitterCursorResult<TItem, TDTO> where TDTO : IBaseCursorQueryDTO<TItem>
    {
        private readonly Func<string, Task<ITwitterResult<TDTO>>> _getItemsFromCursor;
        private bool _isOperationRunning;

        public TwitterCursorResult(Func<string, Task<ITwitterResult<TDTO>>> getItemsFromCursor)
        {
            if (getItemsFromCursor == null)
            {
                throw new ArgumentNullException(nameof(getItemsFromCursor));
            }

            _getItemsFromCursor = getItemsFromCursor;

            TwitterResults = new List<ITwitterResult<TDTO>>();
            Items = new List<TItem>(); 
        }

        public List<ITwitterResult<TDTO>> TwitterResults { get; }
        public List<TItem> Items { get; }
        public TItem[] Current { get; private set; }
        public string NextCursor { get; set; }
        public string PreviousCursor { get; private set; }
        public bool Completed => TwitterResults.Count > 0 && (NextCursor == null || NextCursor == "0");

        public async Task<ITwitterResult<TDTO>> MoveNext(string nextCursor)
        {
            EnsuresASingleOperationRunsAtOnce();

            if (Completed)
            {
                throw new InvalidOperationException("You have already retrieved all the items");
            }

            var twitterResult = await _getItemsFromCursor(nextCursor);
            var dto = twitterResult.DataTransferObject;
            
            TwitterResults.Add(twitterResult);

            if (dto != null)
            {
                var cursorItems = dto.Results?.ToArray();
                
                // only when the request succeeded do we want to move the cursor
                NextCursor = dto.NextCursorStr;
                PreviousCursor = dto.PreviousCursorStr;
                
                // only when the request succeeded do we want to change the current items
                if (cursorItems != null)
                {
                    Current = cursorItems;
                    Items.AddRange(cursorItems);
                }
            }

            _isOperationRunning = false;

            return twitterResult;
        }
        
        public Task<ITwitterResult<TDTO>> MoveNext()
        {
            return MoveNext(NextCursor);
        }

        private void EnsuresASingleOperationRunsAtOnce()
        {
            if (_isOperationRunning)
            {
                throw new InvalidOperationException($"You can only run 1 {nameof(MoveNext)} at a time");
            }

            _isOperationRunning = true;
        }
    }
}
