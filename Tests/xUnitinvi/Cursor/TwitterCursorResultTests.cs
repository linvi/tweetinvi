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
        public void BeforeMoveNext_ShouldNotHaveExecutedCallback()
        {
            var callback = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            var result = new TwitterResult<IdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = new IdsCursorQueryResultDTO
                {
                    Ids = new long[] { 42 },
                    NextCursorStr = "0"
                }
            };

            A.CallTo(callback).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().Returns(result);

            // act
            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(callback);

            // assert
            Assert.False(cursorResult.Completed);

            A.CallTo(callback).MustNotHaveHappened();
        }

        [Fact]
        public async Task MoveNext_RunCallbackOperation()
        {
            var callback = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            var result = new TwitterResult<IdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = new IdsCursorQueryResultDTO
                {
                    Ids = new long[] { 42 },
                    NextCursorStr = "0"
                }
            };

            A.CallTo(callback).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().Returns(result);

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(callback);

            // act
            await cursorResult.MoveToNextPage();

            // assert
            Assert.True(cursorResult.Completed);

            A.CallTo(callback).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task MoveNext_CallbackReturnsNextCursor_ShouldNotBeCompleted()
        {
            var callback = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            var result = new TwitterResult<IdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = new IdsCursorQueryResultDTO
                {
                    Ids = new long[] { 42 },
                    NextCursorStr = "42314",
                }
            };

            A.CallTo(callback).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().Returns(result);

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(callback);

            // act
            await cursorResult.MoveToNextPage();

            // assert
            Assert.False(cursorResult.Completed);

            A.CallTo(callback).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task MoveNext_ThrowsWhenAlreadyCompleted()
        {
            var callback = A.Fake<Func<string, Task<ITwitterResult<IdsCursorQueryResultDTO>>>>();
            var result = new TwitterResult<IdsCursorQueryResultDTO>(null)
            {
                DataTransferObject = new IdsCursorQueryResultDTO
                {
                    Ids = new long[] { 42 },
                    NextCursorStr = "0"
                }
            };

            A.CallTo(callback).WithReturnType<Task<ITwitterResult<IdsCursorQueryResultDTO>>>().Returns(result);

            var cursorResult = new TwitterCursorResult<long, IdsCursorQueryResultDTO>(callback);
            await cursorResult.MoveToNextPage();

            // act - assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await cursorResult.MoveToNextPage());
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
            await cursorResult.MoveToNextPage();

            Assert.False(cursorResult.Completed);
            Assert.Equal(cursorResult.NextCursor, "NextCursor");

            await cursorResult.MoveToNextPage();

            Assert.True(cursorResult.Completed);
            Assert.Equal(cursorResult.NextCursor, "0");

            A.CallTo(fake).MustHaveHappenedTwiceExactly();
        }
    }
}
