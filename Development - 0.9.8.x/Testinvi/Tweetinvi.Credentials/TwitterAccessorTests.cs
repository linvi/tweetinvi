using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Testinvi.Helpers;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;

namespace Testinvi.Tweetinvi.Credentials
{
    public class TwitterAccessorTests
    {
        private List<long> _cursorQueryIds = new List<long>();

        private IIdsCursorQueryResultDTO GenerateIdsCursorQueryResultWithLong()
        {
            var id = TestHelper.GenerateRandomLong();
            long[] ids = { id };
            _cursorQueryIds.Add(id);

            var idsCursorResult = A.Fake<IIdsCursorQueryResultDTO>();
            idsCursorResult.CallsTo(x => x.Ids).Returns(ids);

            return idsCursorResult;
        }
    }
}