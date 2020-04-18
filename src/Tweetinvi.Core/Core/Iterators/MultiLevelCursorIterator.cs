using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;

namespace Tweetinvi.Core.Iterators
{
    public class MultiLevelCursorIterator<TParent, TItem> : MultiLevelCursorIterator<TParent, TItem, string>, IMultiLevelCursorIterator<TParent, TItem>
    {
        public MultiLevelCursorIterator(
            Func<Task<ICursorPageResult<TParent, string>>> iterateSubLevel,
            Func<TParent[], Task<IPageProcessingResult<TParent, TItem>>> getChildItemsPageFromParent) : base(iterateSubLevel, getChildItemsPageFromParent)
        {
        }
    }

    public class MultiLevelCursorIterator<TParent, TItem, TCursor> : IMultiLevelCursorIterator<TParent, TItem, TCursor>
    {
        private readonly Func<Task<ICursorPageResult<TParent, TCursor>>> _iterateSubLevel;
        private readonly Func<TParent[], Task<IPageProcessingResult<TParent, TItem>>> _getChildItemsPageFromParent;

        private HashSet<TParent> _itemsLeftToProcess;
        private ICursorPageResult<TParent, TCursor> _lastParentPageResult;

        public MultiLevelCursorIterator(
            Func<Task<ICursorPageResult<TParent, TCursor>>> iterateSubLevel,
            Func<TParent[], Task<IPageProcessingResult<TParent, TItem>>> getChildItemsPageFromParent)
        {
            _iterateSubLevel = iterateSubLevel;
            _getChildItemsPageFromParent = getChildItemsPageFromParent;
            _itemsLeftToProcess = new HashSet<TParent>();
        }

        public bool Completed => _lastParentPageResult != null && _lastParentPageResult.IsLastPage && _itemsLeftToProcess.Count == 0;

        public async Task<IMultiLevelCursorIteratorPage<TParent, TItem, TCursor>> NextPage()
        {
            if (Completed)
            {
                throw new TwitterIteratorAlreadyCompletedException();
            }

            if (_lastParentPageResult == null || _itemsLeftToProcess.Count == 0)
            {
                _lastParentPageResult = await _iterateSubLevel().ConfigureAwait(false);
                _itemsLeftToProcess = new HashSet<TParent>(_lastParentPageResult.Items);
            }

            var childItemsPage = await _getChildItemsPageFromParent(_itemsLeftToProcess.ToArray()).ConfigureAwait(false);
            var processedParentItems = childItemsPage.AssociatedParentItems;

            processedParentItems.ForEach(item => { _itemsLeftToProcess.Remove(item); });

            var pageResult = new MultiLevelCursorIteratorPage<TParent, TItem, TCursor>
            {
                Items = childItemsPage.Items,
                AssociatedParentItems = processedParentItems,
                NextCursor = _lastParentPageResult.NextCursor,
                IsLastPage = Completed
            };

            return pageResult;
        }
    }
}