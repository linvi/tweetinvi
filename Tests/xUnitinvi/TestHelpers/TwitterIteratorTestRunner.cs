using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.Configuration;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;

namespace xUnitinvi.TestHelpers
{
    public class TwitterIteratorTestRunner<TDTO, TCursor>
    {
        private ITwitterResult<TDTO>[] _expectedResults;
        private readonly ITwitterPageIterator<ITwitterResult<TDTO>, TCursor> _iterator;
        private readonly ITwitterResult<TDTO>[] _results;
        private readonly List<ITwitterIteratorPageResult<ITwitterResult<TDTO>, TCursor>> _resultPages  = new List<ITwitterIteratorPageResult<ITwitterResult<TDTO>, TCursor>>();

        public TwitterIteratorTestRunner(ITwitterPageIterator<ITwitterResult<TDTO>, TCursor> iterator)
        {
            _iterator = iterator;
        }

        public TwitterIteratorTestRunner(
            ITwitterPageIterator<ITwitterResult<TDTO>, TCursor> iterator,
            ITwitterResult<TDTO>[] results)
        {
            _iterator = iterator;
            _results = results;
        }

        protected virtual ITwitterResult<TDTO>[] CreatePages()
        {
            return _results;
        }

        public void Arrange(IReturnValueArgumentValidationConfiguration<Task<ITwitterResult<TDTO>>> callToQueryExecutor)
        {
            if (_expectedResults != null)
            {
                throw new InvalidOperationException("Can only be run once");
            }

            _expectedResults = CreatePages();

            callToQueryExecutor.ReturnsNextFromSequence(_expectedResults);
        }

        public async Task Act()
        {
            if (_iterator.NextCursor != null)
            {
                throw new InvalidOperationException("Cannot run with pre executed iterator");
            }

            while (!_iterator.Completed)
            {
                var page = await _iterator.NextPage();
                _resultPages.Add(page);
            }
        }

        public async Task Assert()
        {
            Xunit.Assert.Equal(_resultPages.Count, _expectedResults.Length);

            for (var i = 0; i < _expectedResults.Length - 1; ++i)
            {
                var page = _resultPages[i];
                Xunit.Assert.Equal(page.Content, _expectedResults[i]);
                Xunit.Assert.False(page.IsLastPage);
            }

            var lastPage = _resultPages[_resultPages.Count - 1];
            Xunit.Assert.Equal(lastPage.Content, _expectedResults[_expectedResults.Length - 1]);
            Xunit.Assert.True(lastPage.IsLastPage);

            await Xunit.Assert.ThrowsAsync<TwitterIteratorAlreadyCompletedException>(() => _iterator.NextPage());
        }
    }
}