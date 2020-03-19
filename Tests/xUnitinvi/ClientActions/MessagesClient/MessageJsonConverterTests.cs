using Tweetinvi;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Models;
using Xunit;

namespace xUnitinvi.ClientActions.MessagesClient
{
    public class MessageJsonConverterTests
    {
        [Fact]
        public void MessageCursorQueryResultDTO_ParsesExpectedly()
        {
            var json = "{\"events\":[],\"next_cursor\":\"MTIzNDk5OTg5MDU0NTA4MjM2OA\"}";
            var result = new TwitterClient(new TwitterCredentials()).Json.Deserialize<MessageCursorQueryResultDTO>(json);

            Assert.Equal(result.NextCursorStr, "MTIzNDk5OTg5MDU0NTA4MjM2OA");
        }
    }
}