using System.Collections.Generic;

namespace Tweetinvi.Iterators
{
    public interface ITwitterIteratorEnumerableResult<out TItem, out TCursor> : IEnumerable<TItem>
    {
        TCursor NextCursor { get; }
        bool IsLastPage { get; }
    }
}