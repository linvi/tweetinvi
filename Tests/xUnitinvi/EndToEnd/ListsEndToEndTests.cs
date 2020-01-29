using System.Linq;
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

            await Task.Delay(1000);
            var updatedPrivateList = await _tweetinviTestClient.Lists.UpdateList(new UpdateListParameters(privateList)
            {
                Name = "new-private-name",
                Description = "hello"
            });

            await Task.Delay(1000);
            var retrievedPrivateList = await _tweetinviTestClient.Lists.GetList(privateList.Id);

            var listFirstCreatedAsPublic = await _tweetinviTestClient.Lists.CreateList("public-endToEnd-Tests", PrivacyMode.Public);

            await Task.Delay(1000);
            var publicListBeforeGoingPrivate = await _tweetinviClient.Lists.GetList(new TwitterListIdentifier(listFirstCreatedAsPublic.Slug, listFirstCreatedAsPublic.Owner));

            var allLists = await _tweetinviTestClient.Lists.GetUserLists();

            await Task.Delay(1000);
            await listFirstCreatedAsPublic.Update(new ListMetadataParameters
            {
                PrivacyMode = PrivacyMode.Private
            });

            // cleanup
            await retrievedPrivateList.Destroy();
            await _tweetinviTestClient.Lists.DestroyList(listFirstCreatedAsPublic);

            // assert
            Assert.Equal(privateList.Name, "private-endToEnd-Tests");
            Assert.Equal(privateList.Description, "private-desc");
            Assert.Equal(privateList.PrivacyMode, PrivacyMode.Private);

            Assert.Equal(updatedPrivateList.Id, privateList.Id);
            Assert.Equal(updatedPrivateList.Description, "hello");

            Assert.Equal(retrievedPrivateList.Name, "new-private-name");
            Assert.Equal(retrievedPrivateList.Description, "hello");

            Assert.Equal(publicListBeforeGoingPrivate.PrivacyMode, PrivacyMode.Public);

            Assert.Equal(listFirstCreatedAsPublic.Name, "public-endToEnd-Tests");
            Assert.Equal(listFirstCreatedAsPublic.PrivacyMode, PrivacyMode.Private);

            Assert.Equal(publicListBeforeGoingPrivate.Name, "public-endToEnd-Tests");

            Assert.Contains(allLists, list => { return list.Id == privateList.Id; });
            Assert.Contains(allLists, list => { return list.Id == publicListBeforeGoingPrivate.Id; });
        }
    }
}