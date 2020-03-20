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
        private readonly TimeSpan _twitterEventualConsistencyDelay = TimeSpan.FromSeconds(1);

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

            await Task.Delay(_twitterEventualConsistencyDelay);
            var updatedPrivateList = await _tweetinviTestClient.Lists.UpdateList(new UpdateListParameters(privateList)
            {
                Name = "new-private-name",
                Description = "hello"
            });

            await Task.Delay(_twitterEventualConsistencyDelay);
            var retrievedPrivateList = await _tweetinviTestClient.Lists.GetList(privateList.Id);

            var listFirstCreatedAsPublic = await _tweetinviTestClient.Lists.CreateList("public-endToEnd-Tests", PrivacyMode.Public);
            await Task.Delay(_twitterEventualConsistencyDelay);
            var publicListBeforeGoingPrivate = await _tweetinviClient.Lists.GetList(new TwitterListIdentifier(listFirstCreatedAsPublic.Slug, listFirstCreatedAsPublic.Owner));

            var listsSubscribedByTweetinviTest = await _tweetinviTestClient.Lists.GetListsSubscribedByAccount();

            await _tweetinviClient.Lists.GetListsOwnedByUser(EndToEndTestConfig.TweetinviTest);
            var listsOwnedByTweetinviTest = await _tweetinviClient.Lists.GetListsOwnedByUserIterator(EndToEndTestConfig.TweetinviTest).MoveToNextPage();

            await Task.Delay(_twitterEventualConsistencyDelay);
            await listFirstCreatedAsPublic.Update(new ListMetadataParameters
            {
                PrivacyMode = PrivacyMode.Private
            });

            // cleanup
            await Task.Delay(_twitterEventualConsistencyDelay);

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
            await Task.Delay(_twitterEventualConsistencyDelay);

            await publicList.AddMember(EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterAdding = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfList(publicList, EndToEndTestConfig.TweetinviApi);
            await _tweetinviTestClient.Lists.AddMemberToList(publicList, EndToEndTestConfig.TweetinviTest);
            await _tweetinviTestClient.Lists.AddMembersToList(publicList, new[] { "bbc", "lemondefr" });

            await _tweetinviClient.Lists.GetMembersOfList(new GetMembersOfListParameters(publicList));
            var membersIterator = _tweetinviTestClient.Lists.GetMembersOfListIterator(new GetMembersOfListParameters(publicList)
            {
                PageSize = 2
            });

            var publicListMembers = new List<IUser>();
            while (!membersIterator.Completed)
            {
                publicListMembers.AddRange(await membersIterator.MoveToNextPage());
            }

            await _tweetinviClient.Lists.GetUserListMemberships(EndToEndTestConfig.TweetinviTest);
            var listsTweetinviTestIsMemberOfIterator = _tweetinviClient.Lists.GetUserListMembershipsIterator(EndToEndTestConfig.TweetinviTest);
            var listsTweetinviTestIsMemberOf = (await listsTweetinviTestIsMemberOfIterator.MoveToNextPage()).ToArray();

            await _tweetinviTestClient.Lists.RemoveMemberFromList(publicList, EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterRemoving = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfList(publicList, EndToEndTestConfig.TweetinviApi);

            await _tweetinviTestClient.Lists.RemoveMembersFromList(publicList, new[] { "bbc", "lemondefr" });
            var updatedList = await _tweetinviTestClient.Lists.GetList(publicList);

            await publicList.Destroy();

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

        [Fact]
        public async Task Subscribers()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _tweetinviTestClient.Lists.CreateList("subscribers-test-list", PrivacyMode.Public);
            await Task.Delay(_twitterEventualConsistencyDelay);

            await _tweetinviClient.Lists.SubscribeToList(publicList);
            await _protectedClient.Lists.SubscribeToList(publicList);
            await _tweetinviTestClient.Lists.SubscribeToList(publicList);

            await _tweetinviClient.Lists.GetListSubscribers(publicList);
            var subscriberIterator = _tweetinviTestClient.Lists.GetListSubscribersIterator(new GetListSubscribersParameters(publicList)
            {
                PageSize = 2
            });

            var subscribers = new List<IUser>();
            while (!subscriberIterator.Completed)
            {
                subscribers.AddRange(await subscriberIterator.MoveToNextPage());
            }

            await _tweetinviClient.Lists.GetUserListSubscriptions(EndToEndTestConfig.TweetinviApi);
            var subscriptionsIterator = _tweetinviTestClient.Lists.GetUserListSubscriptionsIterator(EndToEndTestConfig.TweetinviApi);
            var subscriptions = (await subscriptionsIterator.MoveToNextPage()).ToArray();

            var bbcSubscriber = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfList(publicList, "bbc");
            var tweetinviSubscriberBeforeRemove = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfList(publicList, EndToEndTestConfig.TweetinviApi);

            await _tweetinviClient.Lists.UnsubscribeFromList(publicList);

            var tweetinviSubscriberAfterRemove = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfList(publicList, EndToEndTestConfig.TweetinviApi);

            await publicList.Destroy();

            // assert
            Assert.Equal(subscribers.Count, 3);
            Assert.False(bbcSubscriber);
            Assert.True(tweetinviSubscriberBeforeRemove);
            Assert.False(tweetinviSubscriberAfterRemove);
            Assert.Contains(subscriptions, list => list.Id == publicList.Id);
        }

        [Fact]
        public async Task Tweets()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _protectedClient.Lists.CreateList("members-test-list", PrivacyMode.Public);

            var tweet = await _tweetinviTestClient.Tweets.PublishTweet("Testing that members are working" + Guid.NewGuid());

            await publicList.AddMember("tweetinvitest");
            await Task.Delay(_twitterEventualConsistencyDelay); // give some time to twitter for timeline generation

            // act
            var getTweetsParameters = new GetTweetsFromListParameters(publicList)
            {
                PageSize = 2
            };

            await _protectedClient.Lists.GetTweetsFromList(getTweetsParameters);
            var tweetsIterator = _protectedClient.Lists.GetTweetsFromListIterator(getTweetsParameters);
            var listTweetsPage1 = await tweetsIterator.MoveToNextPage();
            getTweetsParameters.PageSize = 4;
            var listTweetsPage2 = await tweetsIterator.MoveToNextPage();

            await tweet.Destroy();
            await publicList.Destroy();

            // assert
            Assert.Contains(listTweetsPage1, listTweet => listTweet.Id == tweet.Id);
            Assert.Equal(listTweetsPage1.Count(), 2);
            Assert.DoesNotContain(listTweetsPage2, listTweet => listTweet.Id == tweet.Id);
            Assert.Equal(listTweetsPage2.Count(), 4);
        }
    }
}