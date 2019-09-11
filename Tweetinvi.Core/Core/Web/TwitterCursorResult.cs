using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Object iterating over cursored stored items in Twitter
    /// </summary>
    /// <typeparam name="TItem">Type of items returned by the cursor requests</typeparam>
    /// <typeparam name="TDTO">DataTransferObject returned by the cursor requests</typeparam>
    public interface ITwitterCursorResult<TItem, TDTO> : ITwitterCursorResult<TItem, TItem, TDTO> where TDTO : IBaseCursorQueryDTO<TItem>
    {
    }

    public class TwitterCursorResult<TItem, TDTO> : TwitterCursorResult<TItem, TItem, TDTO>, ITwitterCursorResult<TItem, TDTO> where TDTO : IBaseCursorQueryDTO<TItem>
    {
        public TwitterCursorResult(Func<string, Task<ITwitterResult<TDTO>>> getItemsFromCursor) : base(getItemsFromCursor, x => x)
        {
        }
    }

    /// <summary>
    /// Object iterating over cursored stored items in Twitter
    /// </summary>
    /// <typeparam name="TItem">Type of items returned by the cursor requests</typeparam>
    /// <typeparam name="TDTO">DataTransferObject returned by the cursor requests</typeparam>
    public interface ITwitterCursorResult<TItem, TCursorItem, TDTO> where TDTO : IBaseCursorQueryDTO<TCursorItem>
    {
        /// <summary>
        /// Items returned by the last cursor request
        /// </summary>
        TItem[] Current { get; }

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
        /// Move to the next cursor and update the items
        /// </summary>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<IDetailedCursorPageResult<TItem, TDTO>> MoveToNextPage();

        /// <summary>
        /// Move to the defined cursor and update the items
        /// </summary>
        /// <param name="nextCursor">Cursor from where to get items</param>
        /// <returns>Items at the cursor</returns>
        Task<IDetailedCursorPageResult<TItem, TDTO>> MoveToNextPage(string nextCursor);
    }


    public class TwitterCursorResult<TItem, TCursorItem, TDTO> : ITwitterCursorResult<TItem, TCursorItem, TDTO> where TDTO : IBaseCursorQueryDTO<TCursorItem>
    {
        private readonly Func<string, Task<ITwitterResult<TDTO>>> _getItemsFromCursor;
        private readonly Func<TCursorItem, TItem> _transform;
        private bool _isOperationRunning;

        public TwitterCursorResult(Func<string, Task<ITwitterResult<TDTO>>> getItemsFromCursor, Func<TCursorItem, TItem> transform)
        {
            if (getItemsFromCursor == null)
            {
                throw new ArgumentNullException(nameof(getItemsFromCursor));
            }

            _getItemsFromCursor = getItemsFromCursor;
            _transform = transform;
        }

        public TItem[] Current { get; private set; }
        public string NextCursor { get; private set; }
        public string PreviousCursor { get; private set; }
        public bool Completed
        {
            get
            {
                var isFirstRequest = NextCursor == null;
                return !isFirstRequest && NextCursor == "0";
            }
        }

        public async Task<IDetailedCursorPageResult<TItem, TDTO>> MoveToNextPage(string nextCursor)
        {
            EnsuresASingleOperationRunsAtOnce();

            if (Completed)
            {
                throw new TwitterCursorOutOfBoundsException("No more pages available, you have already retrieved all the items.");
            }

            var twitterResult = await _getItemsFromCursor(nextCursor);
            var dto = twitterResult.DataTransferObject;

            TItem[] items = null;

            if (dto != null)
            {
                var cursorItems = dto.Results?.ToArray();

                // only when the request succeeded do we want to move the cursor
                NextCursor = dto.NextCursorStr;
                PreviousCursor = dto.PreviousCursorStr;

                // only when the request succeeded do we want to change the current items
                if (cursorItems != null)
                {
                    items = cursorItems.Select(x => _transform(x)).ToArray();
                    Current = items;
                }
            }

            _isOperationRunning = false;

            return new DetailedCursorPageResult<TItem, TDTO>(twitterResult, items, NextCursor, PreviousCursor);
        }

        public Task<IDetailedCursorPageResult<TItem, TDTO>> MoveToNextPage()
        {
            return MoveToNextPage(NextCursor);
        }

        private void EnsuresASingleOperationRunsAtOnce()
        {
            if (_isOperationRunning)
            {
                throw new InvalidOperationException($"You can only run 1 {nameof(MoveToNextPage)} at a time");
            }

            _isOperationRunning = true;
        }
    }
}
