using System;
using System.Threading.Tasks;
using Tweetinvi.Iterators;

namespace Tweetinvi.Core.Iterators
{
    public class TwitterIteratorProxy<TInput, TOutput> : TwitterIteratorProxy<TInput, TOutput, string>, ITwitterIterator<TOutput>
    {
        public TwitterIteratorProxy(ITwitterPageIterator<TInput, string> source, Func<TInput, TOutput[]> transform) : base(source, transform)
        {
        }
    }

    public class TwitterIteratorProxy<TInput, TOutput, TCursor> : ITwitterIterator<TOutput, TCursor>
    {
        private readonly ITwitterPageIterator<TInput, TCursor> _source;
        private readonly Func<TInput, TOutput[]> _transform;

        public TwitterIteratorProxy(ITwitterPageIterator<TInput, TCursor> source, Func<TInput, TOutput[]> transform)
        {
            _source = source;
            _transform = transform;
        }

        public TCursor NextCursor { get; private set; }
        public bool Completed { get; private set; }

        public async Task<ITwitterIteratorEnumerableResult<TOutput, TCursor>> MoveToNextPage()
        {
            var page = await _source.MoveToNextPage().ConfigureAwait(false);
            var items = _transform(page.Content);

            NextCursor = page.NextCursor;
            Completed = page.IsLastPage;

            return new TwitterIteratorEnumerableResult<TOutput[], TOutput, TCursor>(items, NextCursor, Completed);
        }
    }
}