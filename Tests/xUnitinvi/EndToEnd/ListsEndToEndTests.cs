using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
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
        public async Task List_CRUD()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var privateList = await _tweetinviTestClient.Lists.CreateList(new CreateListParameters("private-endToEnd-Tests")
            {
                PrivacyMode = PrivacyMode.Private,
                Description = "private-desc"
            });

            await Task.Delay(500);
            var updatedPrivateList = await _tweetinviTestClient.Lists.UpdateList(new UpdateListParameters(privateList)
            {
                Name = "new-private-name",
                Description = "hello"
            });

            await Task.Delay(500);
            var retrievedPrivateList = await _tweetinviTestClient.Lists.GetList(privateList.Id);

            var listFirstCreatedAsPublic = await _tweetinviTestClient.Lists.CreateList("public-endToEnd-Tests", PrivacyMode.Public);
            await Task.Delay(500);
            var publicListBeforeGoingPrivate = await _tweetinviClient.Lists.GetList(new TwitterListIdentifier(listFirstCreatedAsPublic.Slug, listFirstCreatedAsPublic.Owner));

            var listsSubscribedByTweetinviTest = await _tweetinviTestClient.Lists.GetListsSubscribedByAccount();
            var listsOwnedByTweetinviTest = await _tweetinviClient.Lists.GetListsOwnedByUserIterator(EndToEndTestConfig.TweetinviTest).MoveToNextPage();

            await Task.Delay(500);
            await listFirstCreatedAsPublic.Update(new ListMetadataParameters
            {
                PrivacyMode = PrivacyMode.Private
            });

            // cleanup
            await Task.Delay(500);

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

            Assert.Contains(listsSubscribedByTweetinviTest, list => { return list.Id == privateList.Id; });
            Assert.Contains(listsSubscribedByTweetinviTest, list => { return list.Id == publicListBeforeGoingPrivate.Id; });
            Assert.Contains(listsOwnedByTweetinviTest, list => { return list.Id == publicListBeforeGoingPrivate.Id; });
        }

        [Fact]
        public async Task Members()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _tweetinviTestClient.Lists.CreateList("members-test-list", PrivacyMode.Public);
            await Task.Delay(500);

            await publicList.AddMember(EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterAdding = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfList(publicList, EndToEndTestConfig.TweetinviApi);
            await _tweetinviTestClient.Lists.AddMemberToList(publicList, EndToEndTestConfig.TweetinviTest);
            await _tweetinviTestClient.Lists.AddMembersToList(publicList, new[] { "bbc", "lemondefr" });

            var membersIterator = _tweetinviTestClient.Lists.GetMembersOfListIterator(new GetMembersOfListParameters(publicList)
            {
                PageSize = 2
            });

            var publicListMembers = new List<IUser>();
            while (!membersIterator.Completed)
            {
                publicListMembers.AddRange(await membersIterator.MoveToNextPage());
            }

            var listsTweetinviTestIsMemberOfIterator = _tweetinviClient.Lists.GetListsAUserIsMemberOfIterator(EndToEndTestConfig.TweetinviTest);
            var listsTweetinviTestIsMemberOf = (await listsTweetinviTestIsMemberOfIterator.MoveToNextPage()).ToArray();

            await _tweetinviTestClient.Lists.RemoveMemberFromList(publicList, EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterRemoving = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfList(publicList, EndToEndTestConfig.TweetinviApi);

            await _tweetinviTestClient.Lists.RemoveMembersFromList(publicList, new[] { "bbc", "lemondefr" });
            var updatedList = await _tweetinviTestClient.Lists.GetList(publicList);

            // await publicList.Destroy();

            // assert
            Assert.Contains(publicListMembers, members => members.ScreenName.ToLower() == EndToEndTestConfig.TweetinviApi);
            Assert.Contains(publicListMembers, members => members.ScreenName.ToLower() == EndToEndTestConfig.TweetinviTest);
            Assert.Contains(publicListMembers, members => members.ScreenName.ToLower() == "bbc");
            Assert.Contains(publicListMembers, members => members.ScreenName.ToLower() == "lemondefr");
            Assert.Contains(listsTweetinviTestIsMemberOf, lists => lists.Id == publicList.Id);
            Assert.True(isTweetinviApiAMemberAfterAdding);
            Assert.False(isTweetinviApiAMemberAfterRemoving);
            Assert.Equal(updatedList.MemberCount, 1);
        }
    }
}