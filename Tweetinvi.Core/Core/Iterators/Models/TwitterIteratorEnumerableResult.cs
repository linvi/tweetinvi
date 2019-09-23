using System.Collections;
using System.Collections.Generic;
using Tweetinvi.Iterators;

namespace Tweetinvi.Core.Iterators
{
    public class TwitterIteratorEnumerableResult<TItemCollection, TItem, TCursor> : ITwitterIteratorEnumerableResult<TItem, TCursor> where TItemCollection : IEnumerable<TItem>
    {
        private readonly TItemCollection _items;

        public TwitterIteratorEnumerableResult(TItemCollection items, TCursor nextCursor, bool isLastPage)
        {
            _items = items;
            
            NextCursor = nextCursor;
            IsLastPage = isLastPage;
        }

        public TCursor NextCursor { get; }
        public bool IsLastPage { get; }

        public IEnumerator<TItem> GetEnumerator()
        {
            return ((IEnumerable<TItem>) _items).GetEnumerator();    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}