using System.Collections.Generic;
using FakeItEasy;
using Testinvi.Helpers;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Testinvi.Tweetinvi.Credentials
{
    public class TwitterAccessorTests
    {
        private readonly List<long> _cursorQueryIds = new List<long>();

        private IIdsCursorQueryResultDTO GenerateIdsCursorQueryResultWithLong()
        {
            var id = TestHelper.GenerateRandomLong();
            long[] ids = { id };
            _cursorQueryIds.Add(id);

            var fakeIdsCursorResult = A.Fake<IIdsCursorQueryResultDTO>();
            A.CallTo(() => fakeIdsCursorResult.Ids).Returns(ids);

            return fakeIdsCursorResult;
        }
    }
}