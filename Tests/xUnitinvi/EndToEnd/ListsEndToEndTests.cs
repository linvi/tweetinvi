using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters.ListsClient;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class ListsEndToEndTests : TweetinviTest
    {
        public ListsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task CRUD()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var privateList = await _tweetinviTestClient.Lists.CreateList(new CreateListParameters("private-endToEnd-Tests")
            {
                PrivacyMode = PrivacyMode.Private,
                Description = "private-desc"
            });

            var publicList = await _tweetinviTestClient.Lists.CreateList("public-endToEnd-Tests", PrivacyMode.Public);
            var retrievedPrivateList = await _tweetinviTestClient.Lists.GetList(privateList.Id);
            var retrievedPublicList = await _tweetinviClient.Lists.GetList(new TwitterListIdentifier(publicList.Slug, publicList.Owner));

            await retrievedPrivateList.Destroy();
            await _tweetinviTestClient.Lists.DestroyList(retrievedPublicList);

            // assert
            Assert.Equal(privateList.Name, "private-endToEnd-Tests");
            Assert.Equal(privateList.Description, "private-desc");
            Assert.Equal(privateList.PrivacyMode, PrivacyMode.Private);


            Assert.Equal(publicList.Name, "public-endToEnd-Tests");
            Assert.Equal(publicList.PrivacyMode, PrivacyMode.Public);

            Assert.Equal(retrievedPrivateList.Name, "private-endToEnd-Tests");
            Assert.Equal(retrievedPublicList.Name, "public-endToEnd-Tests");
        }
    }
}