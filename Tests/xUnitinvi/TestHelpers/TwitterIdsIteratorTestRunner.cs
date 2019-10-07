using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.WebLogic;

namespace xUnitinvi.TestHelpers
{
    public class TwitterIdsIteratorTestRunner : TwitterIteratorTestRunner<IIdsCursorQueryResultDTO, string>
    {
        public TwitterIdsIteratorTestRunner(ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> iterator) : base(iterator)
        {
        }

        protected override ITwitterResult<IIdsCursorQueryResultDTO>[] CreatePages()
        {
            return new ITwitterResult<IIdsCursorQueryResultDTO>[]
            {
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42, 43 },
                        NextCursorStr = "cursor_to_page_2",
                    },
                    Response = new TwitterResponse()
                },
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 44, 45 },
                        NextCursorStr = "0",
                    },
                    Response = new TwitterResponse()
                }
            };
        }
    }
}