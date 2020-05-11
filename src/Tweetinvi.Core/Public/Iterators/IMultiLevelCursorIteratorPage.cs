using Tweetinvi.Core.Iterators;

namespace Tweetinvi.Iterators
{
    /// <summary>
    /// Iterator page containing results from multiple endpoints when using MultiLevelCursorIterator
    /// </summary>
    public interface IMultiLevelCursorIteratorPage<TParent, TItem, TCursor> : ICursorPageResult<TItem, TCursor>
    {
        /// <summary>
        /// Parent items that were transformed into the items
        /// </summary>
        TParent[] AssociatedParentItems { get; set; }
    }
}