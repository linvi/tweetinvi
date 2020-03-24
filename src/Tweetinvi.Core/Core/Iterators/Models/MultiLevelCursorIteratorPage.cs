using Tweetinvi.Iterators;

namespace Tweetinvi.Core.Iterators
{
    public class MultiLevelCursorIteratorPage<TParent, TItem, TCursor> : CursorPageResult<TItem, TCursor>, IMultiLevelCursorIteratorPage<TParent, TItem, TCursor>
    {
        public TParent[] AssociatedParentItems { get; set; }
    }
}