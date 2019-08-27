using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Web;
using Xunit;

namespace xUnitinvi
{
    public class TwitterCursorResultTests
    {
        [Fact]
        public void Ctor_CallbackCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TwitterCursorResult<long, IdsCursorQueryResultDTO>(null));
        }

        [Fact]
        public async Task MoveNext_RunCallbackOperation()
        {
            var fake = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            A.CallTo(fake).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().ReturnsLazily(async (cursor) =>
            {
                return new TwitterResult<IdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42 },
                    }
                };
            });

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(fake);

            // act
            await cursorResult.MoveNext();

            // assert
            Assert.True(cursorResult.Completed);

            A.CallTo(fake).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task MoveNext_ThrowsWhenAlreadyCompleted()
        {
            var fake = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            A.CallTo(fake).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().ReturnsLazily(async (cursor) =>
            {
                return new TwitterResult<IdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42 },
                    }
                };
            });

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(fake);
            await cursorResult.MoveNext();

            // act - assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await cursorResult.MoveNext());
        }

        [Fact]
        public async Task MoveNext_Requires2OperationsToComplete()
        {
            var fake = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            A.CallTo(fake).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().ReturnsLazily(async (call) =>
            {
                var result = new TwitterResult<IdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42 },
                        NextCursorStr = call.Arguments[0] == null ? "NextCursor" : "0"
                    }
                };

                return result;
            });

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(fake);

            // act - assert
            await cursorResult.MoveNext();

            Assert.False(cursorResult.Completed);
            Assert.Equal(cursorResult.NextCursor, "NextCursor");

            await cursorResult.MoveNext();

            Assert.True(cursorResult.Completed);
            Assert.Equal(cursorResult.NextCursor, "0");

            A.CallTo(fake).MustHaveHappenedTwiceExactly();
        }
    }
}
