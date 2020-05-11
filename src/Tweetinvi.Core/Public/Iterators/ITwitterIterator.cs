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
        Task<ITwitterIteratorEnumerableResult<TItem, TCursor>> NextPageAsync();
    }
}