using Tweetinvi.Core.Iterators;

namespace Tweetinvi.Iterators
{
    public interface IMultiLevelCursorIteratorPage<TParent, TItem, TCursor> : ICursorPageResult<TItem, TCursor>
    {
        /// <summary>
        /// Parent items that were transformed into the items
        /// </summary>
        TParent[] AssociatedParentItems { get; set; }
    }
}