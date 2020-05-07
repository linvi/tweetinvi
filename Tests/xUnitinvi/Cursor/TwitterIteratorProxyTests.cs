using System;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Models;
using Xunit;

namespace xUnitinvi.Cursor
{
    public class TwitterIteratorProxyTests
    {
        public TwitterIteratorProxyTests()
        {
            _page1UserIds = new long[] { 42, 43 };
            _page2UserIds = new long[] { 44, 45 };

            _source = A.Fake<ITwitterPageIterator<IUserIdentifier[], string>>();
            _transform = A.Fake<Func<IUserIdentifier[], long[]>>();

            var page1UserIdentifiers = _page1UserIds.Select(id => new UserIdentifier(id)).Cast<IUserIdentifier>().ToArray();
            var firstPage = A.Fake<ITwitterIteratorPageResult<IUserIdentifier[], string>>();
            A.CallTo(() => firstPage.NextCursor).Returns("cursor_for_page_2");
            A.CallTo(() => firstPage.IsLastPage).Returns(false);
            A.CallTo(() => firstPage.Content).Returns(page1UserIdentifiers);
            A.CallTo(() => _transform(page1UserIdentifiers)).Returns(_page1UserIds);

            var page2UserIdentifiers = _page1UserIds.Select(id => new UserIdentifier(id)).Cast<IUserIdentifier>().ToArray();
            var secondPage = A.Fake<ITwitterIteratorPageResult<IUserIdentifier[], string>>();
            A.CallTo(() => secondPage.NextCursor).Returns("0");
            A.CallTo(() => secondPage.IsLastPage).Returns(true);
            A.CallTo(() => secondPage.Content).Returns(page2UserIdentifiers);
            A.CallTo(() => _transform(page2UserIdentifiers)).Returns(_page2UserIds);

            A.CallTo(() => _source.NextPageAsync()).ReturnsNextFromSequence(firstPage, secondPage);
        }

        private readonly long[] _page1UserIds;
        private readonly long[] _page2UserIds;
        private readonly ITwitterPageIterator<IUserIdentifier[], string> _source;
        private readonly Func<IUserIdentifier[], long[]> _transform;

        [Fact]
        public async Task NextPage_ReturnsFirstPageAsync()
        {
            // arrange
            var proxyIterator = new TwitterIteratorProxy<IUserIdentifier[], long, string>(_source, _transform);

            // act
            var page = await proxyIterator.NextPageAsync();

            // assert
            Assert.False(page.IsLastPage);
            Assert.False(proxyIterator.Completed);

            Assert.Equal("cursor_for_page_2", page.NextCursor);
            Assert.Equal("cursor_for_page_2", proxyIterator.NextCursor);

            Assert.Equal(_page1UserIds, page.ToArray());
        }

        [Fact]
        public async Task NextPageSecondTimes_ReturnsSecondPageAsync()
        {
            // arrange
            var proxyIterator = new TwitterIteratorProxy<IUserIdentifier[], long, string>(_source, _transform);

            // 1st iteration
            await proxyIterator.NextPageAsync();

            // act
            var page = await proxyIterator.NextPageAsync();

            // assert
            Assert.True(page.IsLastPage);
            Assert.True(proxyIterator.Completed);

            Assert.Equal("0", page.NextCursor);
            Assert.Equal("0", proxyIterator.NextCursor);

            Assert.Equal(_page2UserIds, page.ToArray());
        }
    }
}