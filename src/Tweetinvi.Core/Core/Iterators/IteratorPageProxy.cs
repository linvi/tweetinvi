using System;
using System.Threading.Tasks;
using Tweetinvi.Iterators;

namespace Tweetinvi.Core.Iterators
{
    public class IteratorPageProxy<TInput, TOutput, TCursor> : ITwitterRequestIterator<TOutput, TCursor>
    {
        private readonly ITwitterPageIterator<TInput, TCursor> _source;
        private readonly Func<TInput, TOutput> _transform;

        public IteratorPageProxy(ITwitterPageIterator<TInput, TCursor> source, Func<TInput, TOutput> transform)
        {
            _source = source;
            _transform = transform;
        }

        public TCursor NextCursor { get; private set; }
        public bool Completed { get; private set; }
        public async Task<IIteratorPageResult<TOutput, TCursor>> NextPageAsync()
        {
            var page = await _source.NextPageAsync().ConfigureAwait(false);
            var output = _transform(page.Content);

            NextCursor = page.NextCursor;
            Completed = page.IsLastPage;

            return new IteratorPageResult<TOutput, TCursor>(output, page.NextCursor, page.IsLastPage);
        }
    }
}