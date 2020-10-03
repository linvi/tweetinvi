using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;

namespace Tweetinvi.Iterators
{
    public interface ITwitterRequestIterator<TPage, TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
        Task<IIteratorPageResult<TPage, TCursor>> NextPageAsync();
    }
}