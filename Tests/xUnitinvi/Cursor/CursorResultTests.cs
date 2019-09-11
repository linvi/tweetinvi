using System;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using FakeItEasy;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.QueryDTO;
using Xunit;

namespace xUnitinvi
{
    public class CursorResultTests
    {
        [Fact]
        public void Ctor_twitterCursorResultCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(null));
        }

        [Fact]
        public async Task MoveNext_WithoutArgs_ShouldMoveNextTheTwitterCursorResult()
        {
            // arrange
            var twitterCursorResult = A.Fake<ITwitterCursorResult<long, IIdsCursorQueryResultDTO>>();
            var cursorResult = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            var twitterResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();
            var idsCursorQueryResultDTO = new IdsCursorQueryResultDTO
            {
                Ids = new long[] { 42 },
                NextCursorStr = "NextCursor",
                PreviousCursorStr = "PreviousCursor"
            };

            A.CallTo(() => twitterCursorResult.MoveToNextPage())
                .ReturnsLazily(() => new DetailedCursorPageResult<long, IIdsCursorQueryResultDTO>(twitterResult, idsCursorQueryResultDTO)
);

            // act
            var result = await cursorResult.MoveToNextPage();

            // assert
            A.CallTo(() => twitterCursorResult.MoveToNextPage()).MustHaveHappenedOnceExactly();

            result.ShouldDeepEqual(new CursorPageResult<long>
            {
                Items = idsCursorQueryResultDTO.Ids,
                NextCursor = idsCursorQueryResultDTO.NextCursorStr,
                PreviousCursor = idsCursorQueryResultDTO.PreviousCursorStr,
                IsLastPage = false
            });
        }

        [Fact]
        public async Task MoveNext_WithToken_ShouldMoveNextTheTwitterCursorResult()
        {
            // arrange
            var twitterCursorResult = A.Fake<ITwitterCursorResult<long, IIdsCursorQueryResultDTO>>();
            var cursorResult = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            var idsCursorQueryResultDTO = new IdsCursorQueryResultDTO
            {
                Ids = new long[] { 42 },
                NextCursorStr = "NextCursor",
                PreviousCursorStr = "PreviousCursor"
            };

            var twitterResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();
            var pageResult = new DetailedCursorPageResult<long, IIdsCursorQueryResultDTO>(twitterResult,
                idsCursorQueryResultDTO.Ids, 
                idsCursorQueryResultDTO.NextCursorStr,
                idsCursorQueryResultDTO.PreviousCursorStr);

            A.CallTo(() => twitterCursorResult.MoveToNextPage("nextToken")).Returns(pageResult);

            // act
            var result = await cursorResult.MoveToNextPage("nextToken");

            // assert
            A.CallTo(() => twitterCursorResult.MoveToNextPage("nextToken")).MustHaveHappenedOnceExactly();

            result.ShouldDeepEqual(new CursorPageResult<long>
            {
                Items = idsCursorQueryResultDTO.Ids,
                NextCursor = idsCursorQueryResultDTO.NextCursorStr,
                PreviousCursor = idsCursorQueryResultDTO.PreviousCursorStr,
                IsLastPage = false
            });
        }
    }
}