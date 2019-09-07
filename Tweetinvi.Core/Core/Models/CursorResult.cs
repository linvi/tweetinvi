using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
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
        TItem[] Items { get; }
        
        /// <summary>
        /// Items returned by the last cursor request
        /// </summary>
        TItem[] LastIterationItems { get; }

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
        public TItem[] Items => _twitterCursorResult.Items.ToArray();
        public TItem[] LastIterationItems => _twitterCursorResult.Current;
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

    public class CursorResult<TItem, TDTOItem, TDTO> : ICursorResult<TItem> where TDTO : IBaseCursorQueryDTO<TDTOItem>
    {
        private readonly ITwitterCursorResult<TDTOItem, TDTO> _twitterCursorResult;
        private readonly Func<TDTOItem[], Task<TItem[]>> _transformer;
        private readonly List<TItem> _items;

        public CursorResult(ITwitterCursorResult<TDTOItem, TDTO> twitterCursorResult, Func<TDTOItem[], Task<TItem[]>> transformer)
        {
            if (twitterCursorResult == null)
            {
                throw new ArgumentNullException(nameof(twitterCursorResult));
            }

            _twitterCursorResult = twitterCursorResult;
            _transformer = transformer;
            _items = new List<TItem>();

            NextCursor = _twitterCursorResult.NextCursor;
            PreviousCursor = _twitterCursorResult.PreviousCursor;
        }

        public string NextCursor { get; private set; }
        public string PreviousCursor { get; private set; }
        public bool Completed { get; private set; }
        public TItem[] Items => _items.ToArray();
        public TItem[] LastIterationItems { get; private set; }

        public Task<CursorOperationResult<TItem>> MoveNext()
        {
            return MoveNext(NextCursor);
        }

        public async Task<CursorOperationResult<TItem>> MoveNext(string nextCursor)
        {
            var result = await _twitterCursorResult.MoveNext(nextCursor);
            return await CreateCursorOperationResult(result);
        }

        private async Task<CursorOperationResult<TItem>> CreateCursorOperationResult(ITwitterResult<TDTO> twitterResults)
        {
            var cursorResult = twitterResults.DataTransferObject;

            var dtoItems = cursorResult.Results.ToArray();
            var items = await _transformer(dtoItems);

            NextCursor = _twitterCursorResult.NextCursor;
            PreviousCursor = _twitterCursorResult.PreviousCursor;

            _items.AddRangeSafely(items);
            LastIterationItems = items;
            Completed = _twitterCursorResult.Completed;

            return new CursorOperationResult<TItem>
            {
                Items = items,
                NextCursor = cursorResult.NextCursorStr,
                PreviousCursor = cursorResult.PreviousCursorStr
            };
        }
    }
}
