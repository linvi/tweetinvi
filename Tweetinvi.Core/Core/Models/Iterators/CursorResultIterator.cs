using System;
using System.Collections.Generic;
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
    public interface ICursorResultIterator<TItem>
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
        /// Items returned by the last cursor request
        /// </summary>
        TItem[] Current { get; }

        /// <summary>
        /// Move to the next cursor and update the items
        /// </summary>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<CursorPageResult<TItem>> MoveToNextPage();
    }
    
    /// <summary>
    /// Object iterating over cursored stored items in Twitter
    /// </summary>
    /// <typeparam name="TItem">Type of items returned by the cursor requests</typeparam>
    public interface ISkippableResultIterator<TItem> : ICursorResultIterator<TItem>
    {
        /// <summary>
        /// Move to the next cursor and update the items
        /// </summary>
        /// <param name="nextCursor">Cursor from where to get items</param>
        /// <returns>TwitterResult of the cursor requests</returns>
        Task<CursorPageResult<TItem>> MoveToNextPage(string nextCursor);
    }

    public class SkippableResultIterator<TItem, TDTO> : ISkippableResultIterator<TItem> where TDTO : IBaseCursorQueryDTO<TItem>
    {
        private readonly ITwitterCursorResult<TItem, TDTO> _twitterCursorResult;

        public SkippableResultIterator(ITwitterCursorResult<TItem, TDTO> twitterCursorResult)
        {
            if (twitterCursorResult == null)
            {
                throw new ArgumentNullException(nameof(twitterCursorResult));
            }
            
            _twitterCursorResult = twitterCursorResult;
        }

        public string PreviousCursor => _twitterCursorResult.PreviousCursor;
        public string NextCursor => _twitterCursorResult.NextCursor;
        public TItem[] Current => _twitterCursorResult.Current;
        public bool Completed => _twitterCursorResult.Completed;

        public async Task<CursorPageResult<TItem>> MoveToNextPage()
        {
            var nextTwitterResults = await _twitterCursorResult.MoveToNextPage().ConfigureAwait(false);
            return CreatePageResult(nextTwitterResults);
        }
        
        public async Task<CursorPageResult<TItem>> MoveToNextPage(string nextCursor)
        {
            var nextTwitterResults = await _twitterCursorResult.MoveToNextPage(nextCursor).ConfigureAwait(false);
            return CreatePageResult(nextTwitterResults);
        }

        private static CursorPageResult<TItem> CreatePageResult(IDetailedCursorPageResult<TItem, TDTO> detailedCursorPageResults)
        {
            return new CursorPageResult<TItem>
            {
                Items = detailedCursorPageResults.Items,
                NextCursor = detailedCursorPageResults.NextCursor,
                PreviousCursor = detailedCursorPageResults.PreviousCursor,
                IsLastPage = detailedCursorPageResults.IsLastPage
            };
        }
    }

    public class SkippableResultIterator<TItem, TDTOItem, TDTO> : ISkippableResultIterator<TItem> where TDTO : IBaseCursorQueryDTO<TDTOItem>
    {
        private readonly ITwitterCursorResult<TDTOItem, TDTO> _twitterCursorDTOResult;
        private readonly ITwitterCursorResult<TItem, TDTOItem, TDTO> _twitterCursorItemResult;

        private readonly Func<TDTOItem[], TItem[]> _transformer;

        public SkippableResultIterator(ITwitterCursorResult<TDTOItem, TDTO> twitterCursorDTOResult, Func<TDTOItem[], TItem[]> transformer)
        {
            if (twitterCursorDTOResult == null)
            {
                throw new ArgumentNullException(nameof(twitterCursorDTOResult));
            }

            _twitterCursorDTOResult = twitterCursorDTOResult;
            _transformer = transformer;
        }

        public SkippableResultIterator(ITwitterCursorResult<TItem, TDTOItem, TDTO> twitterCursorItemResult)
        {
            _twitterCursorItemResult = twitterCursorItemResult;
            
            NextCursor = _twitterCursorDTOResult.NextCursor;
            PreviousCursor = _twitterCursorDTOResult.PreviousCursor;
        }

        public string NextCursor { get; private set; }
        public string PreviousCursor { get; private set; }
        public bool Completed { get; private set; }
        public TItem[] Current { get; private set; }

        public Task<CursorPageResult<TItem>> MoveToNextPage()
        {
            return MoveToNextPage(NextCursor);
        }

        public async Task<CursorPageResult<TItem>> MoveToNextPage(string nextCursor)
        {
            if (_transformer != null)
            {
                var result = await _twitterCursorDTOResult.MoveToNextPage(nextCursor).ConfigureAwait(false);
                var items = _transformer(result.Items);
                return CursorOperationResult(items, result.NextCursor, result.PreviousCursor);
            }
            else
            {
                var result = await _twitterCursorItemResult.MoveToNextPage(nextCursor).ConfigureAwait(false);
                return CursorOperationResult(result.Items, result.NextCursor, result.PreviousCursor);
            }
        }

        private CursorPageResult<TItem> CursorOperationResult(TItem[] items, string nextCursor, string previousCursor)
        {
            NextCursor = _twitterCursorDTOResult.NextCursor;
            PreviousCursor = _twitterCursorDTOResult.PreviousCursor;

            Current = items;
            Completed = _twitterCursorDTOResult.Completed;

            return new CursorPageResult<TItem>
            {
                Items = items,
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
                IsLastPage = Completed
            };
        }
    }
}
