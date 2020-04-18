using System;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Xunit;

namespace xUnitinvi.Cursor
{
    public class MultiLevelCursorIteratorTests
    {
        public MultiLevelCursorIteratorTests()
        {
            var page1 = new CursorPageResult<long, string>
            {
                Items = new long[] { 42, 43 },
                NextCursor = "cursor_for_page_2",
                IsLastPage = false
            };

            var page2 = new CursorPageResult<long, string>
            {
                Items = new long[] { 44, 45 },
                NextCursor = "cursor_for_page_3",
                IsLastPage = true
            };

            _getPageFunc = A.Fake<Func<Task<ICursorPageResult<long, string>>>>();
            _page1ProcessingResult = new MultiLevelPageProcessingResult<long, IUserIdentifier>
            {
                AssociatedParentItems = page1.Items,
                Items = page1.Items.Select(id => new UserIdentifier(id)).Cast<IUserIdentifier>().ToArray()
            };

            _page2FirstProcessingResult = new MultiLevelPageProcessingResult<long, IUserIdentifier>
            {
                AssociatedParentItems = page2.Take(1).ToArray(),
                Items = page2.Take(1).Select(id => new UserIdentifier(id)).Cast<IUserIdentifier>().ToArray()
            };

            _page2SecondProcessingResult = new MultiLevelPageProcessingResult<long, IUserIdentifier>
            {
                AssociatedParentItems = page2.Skip(1).Take(1).ToArray(),
                Items = page2.Skip(1).Take(1).Select(id => new UserIdentifier(id)).Cast<IUserIdentifier>().ToArray()
            };

            A.CallTo(() => _getPageFunc()).ReturnsNextFromSequence(page1, page2);

            _getUserIdentifiersFunc = A.Fake<Func<long[], Task<IPageProcessingResult<long, IUserIdentifier>>>>();

            A.CallTo(() => _getUserIdentifiersFunc(A<long[]>.That.IsSameSequenceAs(page1.Items))).Returns(_page1ProcessingResult);
            A.CallTo(() => _getUserIdentifiersFunc(A<long[]>.That.IsSameSequenceAs(page2.Items))).Returns(_page2FirstProcessingResult);
            A.CallTo(() => _getUserIdentifiersFunc(A<long[]>.That.IsSameSequenceAs(page2.Items.Skip(1).Take(1)))).Returns(_page2SecondProcessingResult);
        }

        private readonly Func<Task<ICursorPageResult<long, string>>> _getPageFunc;
        private readonly Func<long[], Task<IPageProcessingResult<long, IUserIdentifier>>> _getUserIdentifiersFunc;
        private readonly MultiLevelPageProcessingResult<long, IUserIdentifier> _page1ProcessingResult;
        private readonly MultiLevelPageProcessingResult<long, IUserIdentifier> _page2FirstProcessingResult;
        private readonly MultiLevelPageProcessingResult<long, IUserIdentifier> _page2SecondProcessingResult;

        [Fact]
        public async Task NextPage_ShouldIterateBothAtParentChildLevel()
        {
            // arrange
            var iterator = new MultiLevelCursorIterator<long, IUserIdentifier, string>(_getPageFunc, _getUserIdentifiersFunc);

            // act
            var firstPage = await iterator.NextPage();
            var secondPage = await iterator.NextPage();
            var thirdPage = await iterator.NextPage();

            // assert
            Assert.Equal(firstPage.Items, _page1ProcessingResult.Items);
            Assert.False(firstPage.IsLastPage);

            Assert.Equal(secondPage.Items, _page2FirstProcessingResult.Items);
            Assert.False(secondPage.IsLastPage);

            Assert.Equal(thirdPage.Items, _page2SecondProcessingResult.Items);
            Assert.True(thirdPage.IsLastPage);

            Assert.True(iterator.Completed);
        }

        [Fact]
        public async Task NextPage_ShouldThrowIfAlreadyCompleted()
        {
            // arrange
            var iterator = new MultiLevelCursorIterator<long, IUserIdentifier, string>(_getPageFunc, _getUserIdentifiersFunc);

            // act
            await iterator.NextPage();
            await iterator.NextPage();
            await iterator.NextPage();

            // act assert
            await Assert.ThrowsAsync<TwitterIteratorAlreadyCompletedException>(() => iterator.NextPage());
        }
    }
}