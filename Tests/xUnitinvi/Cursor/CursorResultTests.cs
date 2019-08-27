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
            Assert.Throws<ArgumentNullException>(() => new CursorResult<long, IIdsCursorQueryResultDTO>(null));
        }

        [Fact]
        public async Task MoveNext_WithoutArgs_ShouldMoveNextTheTwitterCursorResult()
        {
            // arrange
            var twitterCursorResult = A.Fake<ITwitterCursorResult<long, IIdsCursorQueryResultDTO>>();
            var cursorResult = new CursorResult<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            var idsCursorQueryResultDTO = new IdsCursorQueryResultDTO()
            {
                Ids = new long[] { 42 },
                NextCursorStr = "NextCursor",
                PreviousCursorStr = "PreviousCursor"
            };

            A.CallTo(() => twitterCursorResult.MoveNext()).Returns(new TwitterResult<IIdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = idsCursorQueryResultDTO
            });

            // act
            var result = await cursorResult.MoveNext();

            // assert
            A.CallTo(() => twitterCursorResult.MoveNext()).MustHaveHappenedOnceExactly();

            result.ShouldDeepEqual(new CursorOperationResult<long>()
            {
                Items = idsCursorQueryResultDTO.Ids,
                NextCursor = idsCursorQueryResultDTO.NextCursorStr,
                PreviousCursor = idsCursorQueryResultDTO.PreviousCursorStr
            });
        }

        [Fact]
        public async Task MoveNext_WithToken_ShouldMoveNextTheTwitterCursorResult()
        {
            // arrange
            var twitterCursorResult = A.Fake<ITwitterCursorResult<long, IIdsCursorQueryResultDTO>>();
            var cursorResult = new CursorResult<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            var idsCursorQueryResultDTO = new IdsCursorQueryResultDTO()
            {
                Ids = new long[] { 42 },
                NextCursorStr = "NextCursor",
                PreviousCursorStr = "PreviousCursor"
            };

            A.CallTo(() => twitterCursorResult.MoveNext("nextToken")).Returns(new TwitterResult<IIdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = idsCursorQueryResultDTO
            });

            // act
            var result = await cursorResult.MoveNext("nextToken");

            // assert
            A.CallTo(() => twitterCursorResult.MoveNext("nextToken")).MustHaveHappenedOnceExactly();

            result.ShouldDeepEqual(new CursorOperationResult<long>()
            {
                Items = idsCursorQueryResultDTO.Ids,
                NextCursor = idsCursorQueryResultDTO.NextCursorStr,
                PreviousCursor = idsCursorQueryResultDTO.PreviousCursorStr
            });
        }
    }
}