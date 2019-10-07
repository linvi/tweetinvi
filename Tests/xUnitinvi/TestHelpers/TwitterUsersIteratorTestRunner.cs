using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryDTO;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.WebLogic;

namespace xUnitinvi.TestHelpers
{
    public class TwitterUsersIteratorTestRunner : TwitterIteratorTestRunner<IUserCursorQueryResultDTO, string>
    {
        public TwitterUsersIteratorTestRunner(ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>, string> iterator) : base(iterator)
        {
        }

        protected override ITwitterResult<IUserCursorQueryResultDTO>[] CreatePages()
        {
            return new ITwitterResult<IUserCursorQueryResultDTO>[]
            {
                new TwitterResult<IUserCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new UserCursorQueryResultDTO
                    {
                        Users = new IUserDTO[]
                        {
                            new UserDTO { Id = 42 },
                            new UserDTO { Id = 43 },
                        },
                        NextCursorStr = "cursor_to_page_2",
                    },
                    Response = new TwitterResponse()
                },
                new TwitterResult<IUserCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new UserCursorQueryResultDTO
                    {
                        Users = new IUserDTO[]
                        {
                            new UserDTO { Id = 44 },
                            new UserDTO { Id = 45 },
                        },
                        NextCursorStr = "0",
                    },
                    Response = new TwitterResponse()
                }
            };
        }
    }
}