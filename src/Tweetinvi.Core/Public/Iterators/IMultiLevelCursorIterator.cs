using System.Threading.Tasks;

namespace Tweetinvi.Iterators
{
    public interface IMultiLevelCursorIterator<TParent, TItem> : IMultiLevelCursorIterator<TParent, TItem, string>
    {
    }

    /// <summary>
    /// Allow developers to iterate over multiple endpoints transparently.
    /// </summary>
    public interface IMultiLevelCursorIterator<TParent, TItem, TCursor>
    {
        bool Completed { get; }
        Task<IMultiLevelCursorIteratorPage<TParent, TItem, TCursor>> NextPageAsync();
    }
}