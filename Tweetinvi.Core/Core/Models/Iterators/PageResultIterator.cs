using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Models
{
    public class PageResultIterator<TInput, TResult>
    {
        private readonly TInput[] _input;
        private readonly Func<TInput[], Task<TResult[]>> _transform;
        private readonly int _maxItemsPerRequest;
        private int _currentPosition;

        public PageResultIterator(TInput[] input, Func<TInput[], Task<TResult[]>> transform, int maxItemsPerRequest)
        {
            _input = input;
            _transform = transform;
            _maxItemsPerRequest = maxItemsPerRequest;
        }

        public bool Completed => _currentPosition >= _input.Length;
        
        public async Task<IPageResult<TResult>> MoveToNextPage()
        {
            if (_currentPosition >= _input.Length)
            {
                return new PageResult<TResult>
                {
                    Items = new TResult[0],
                    IsLastPage = true
                };
            }
            
            var pageItemsInput = _input.Skip(_currentPosition).Take(_maxItemsPerRequest).ToArray();

            var pageResults = await _transform(pageItemsInput).ConfigureAwait(false);

            if (pageResults == null)
            {
                return new PageResult<TResult>
                {
                    Items = new TResult[0],
                    IsLastPage = false
                };
            }

            _currentPosition += _maxItemsPerRequest;
                
            return new PageResult<TResult>
            {
                Items = pageResults,
                IsLastPage = _currentPosition >= _input.Length,
            };
        }
    }
}