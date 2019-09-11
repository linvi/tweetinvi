using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Models
{
    public class MultiLevelCursorResultIterator<TItem, TParentCursorItem> : ICursorResultIterator<TItem>
    {
        private PageResultIterator<TParentCursorItem, TItem> _cursorResultIterator;

        private readonly ICursorResultIterator<TParentCursorItem> _childCursorResultIterator;
        private readonly Func<TParentCursorItem[], PageResultIterator<TParentCursorItem, TItem>> _getCursorIterator;

        public MultiLevelCursorResultIterator(
            ICursorResultIterator<TParentCursorItem> childCursorResultIterator,
            Func<TParentCursorItem[], PageResultIterator<TParentCursorItem, TItem>> getCursorIterator)
        {
            _childCursorResultIterator = childCursorResultIterator;
            _getCursorIterator = getCursorIterator;
        }

        public string NextCursor { get; private set; }
        public string PreviousCursor { get; private set; }
        public bool Completed => _childCursorResultIterator?.Completed == true && _cursorResultIterator?.Completed == true;
        public TItem[] Current { get; private set; }
        
        public async Task<CursorPageResult<TItem>> MoveToNextPage()
        {
            if (Completed)
            {
                throw new InvalidOperationException("You have already retrieved all the items");
            }
            
            if (_cursorResultIterator?.Completed == null || _cursorResultIterator?.Completed == true)
            {
                var nextChildPageItems = await _childCursorResultIterator.MoveToNextPage().ConfigureAwait(false);

                NextCursor = nextChildPageItems.NextCursor;
                PreviousCursor = nextChildPageItems.PreviousCursor;

                _cursorResultIterator = _getCursorIterator(nextChildPageItems.Items);
            }

            var pageItems = await _cursorResultIterator.MoveToNextPage();

            Current = pageItems.ToArray();

            return new CursorPageResult<TItem>
            {
                Items = Current,
                NextCursor = NextCursor,
                PreviousCursor = PreviousCursor,
                IsLastPage = Completed
            };
        }
    }
}