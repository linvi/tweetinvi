using System.Threading.Tasks;

namespace Tweetinvi.Iterators
{
    /// <summary>
    /// Iterator allowing you to navigate through Twitter API pages
    /// </summary>
    public interface ITwitterIterator<TItem> : ITwitterIterator<TItem, string>
    {
    }

    /// <summary>
    /// Iterator allowing you to navigate through Twitter API pages
    /// </summary>
    public interface ITwitterIterator<TItem, TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
        Task<ITwitterIteratorEnumerablePage<TItem, TCursor>> NextPageAsync();
    }

    public interface ITwitterSimpleIteratorResult<TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
    }

    public interface ITwitterSimpleIterator<TItem, TCursor> where TItem : ITwitterSimpleIteratorResult<TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
        Task<TItem> NextPageAsync();
    }
}