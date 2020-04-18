using System.Threading.Tasks;

namespace Tweetinvi.Iterators
{
    public interface ITwitterIterator<TItem> : ITwitterIterator<TItem, string>
    {
    }

    public interface ITwitterIterator<TItem, TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
        Task<ITwitterIteratorEnumerableResult<TItem, TCursor>> NextPage();
    }
}