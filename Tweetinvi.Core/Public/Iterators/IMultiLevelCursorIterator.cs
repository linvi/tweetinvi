using System.Threading.Tasks;

namespace Tweetinvi.Iterators
{
    public interface IMultiLevelCursorIterator<TParent, TItem> : IMultiLevelCursorIterator<TParent, TItem, string>
    {
    }

    public interface IMultiLevelCursorIterator<TParent, TItem, TCursor>
    {
        bool Completed { get; }
        Task<IMultiLevelCursorIteratorPage<TParent, TItem, TCursor>> MoveToNextPage();
    }
}