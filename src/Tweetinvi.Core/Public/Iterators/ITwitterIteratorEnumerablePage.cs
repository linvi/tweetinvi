using System.Collections.Generic;

namespace Tweetinvi.Iterators
{
    /// <summary>
    /// An iterator page containing the values of a specific page.
    /// It also give access to the next page as well as informing if a next page exists.
    /// </summary>
    public interface ITwitterIteratorEnumerablePage<out TItem, out TCursor> : ITwitterIteratorPage<TCursor>, IEnumerable<TItem>
    {
    }
}