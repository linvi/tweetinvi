using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;

namespace Tweetinvi.Core.Iterators
{
    public interface ITwitterPageIterator<TPage, TCursor>
    {
        TCursor NextCursor { get; }
        bool Completed { get; }
        Task<ITwitterIteratorPageResult<TPage, TCursor>> NextPageAsync();
    }

    public interface ITwitterPageIterator<TPage> : ITwitterPageIterator<TPage, string>
    {
    }

    public class TwitterPageIterator<TPage> : TwitterPageIterator<TPage, string>, ITwitterPageIterator<TPage> where TPage : ITwitterResult
    {
        public TwitterPageIterator(
            string initialCursor,
            Func<string, Task<TPage>> getNextPage,
            Func<TPage, string> extractNextCursor,
            Func<TPage, bool> isCompleted)
            : base(initialCursor, getNextPage, extractNextCursor, isCompleted)
        {
        }
    }

    public class TwitterPageIterator<TPage, TCursor> : ITwitterPageIterator<TPage, TCursor> where TPage : ITwitterResult
    {
        private readonly Func<TCursor, Task<TPage>> _getNextPage;
        private readonly Func<TPage, TCursor> _extractNextCursor;
        private readonly Func<TPage, bool> _isCompleted;

        public TwitterPageIterator(
            TCursor initialCursor,
            Func<TCursor, Task<TPage>> getNextPage,
            Func<TPage, TCursor> extractNextCursor,
            Func<TPage, bool> isCompleted)
        {
            NextCursor = initialCursor;

            _getNextPage = getNextPage;
            _extractNextCursor = extractNextCursor;
            _isCompleted = isCompleted;
        }

        public TCursor NextCursor { get; private set; }
        public bool Completed { get; private set; }

        public async Task<ITwitterIteratorPageResult<TPage, TCursor>> NextPageAsync()
        {
            if (Completed)
            {
                throw new TwitterIteratorAlreadyCompletedException();
            }

            var page = await _getNextPage(NextCursor).ConfigureAwait(false);
            NextCursor = _extractNextCursor(page);
            Completed = _isCompleted(page);

            return new TwitterIteratorPageResult<TPage, TCursor>(page, NextCursor, Completed);
        }
    }
}