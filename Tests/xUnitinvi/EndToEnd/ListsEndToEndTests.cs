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
        public async Task List_CRUDAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var privateList = await _tweetinviTestClient.Lists.CreateListAsync(new CreateListParameters("private-endToEnd-Tests")
            {
                PrivacyMode = PrivacyMode.Private,
                Description = "private-desc"
            });

            await Task.Delay(_twitterEventualConsistencyDelay);
            var updatedPrivateList = await _tweetinviTestClient.Lists.UpdateListAsync(new UpdateListParameters(privateList)
            {
                Name = "new-private-name",
                Description = "hello"
            });

            await Task.Delay(_twitterEventualConsistencyDelay);
            var retrievedPrivateList = await _tweetinviTestClient.Lists.GetListAsync(privateList.Id);

            var listFirstCreatedAsPublic = await _tweetinviTestClient.Lists.CreateListAsync("public-endToEnd-Tests", PrivacyMode.Public);
            await Task.Delay(_twitterEventualConsistencyDelay);
            var publicListBeforeGoingPrivate = await _tweetinviClient.Lists.GetListAsync(new TwitterListIdentifier(listFirstCreatedAsPublic.Slug, listFirstCreatedAsPublic.Owner));

            var listsSubscribedByTweetinviTest = await _tweetinviTestClient.Lists.GetListsSubscribedByAccountAsync();

            await _tweetinviClient.Lists.GetListsOwnedByUserAsync(EndToEndTestConfig.TweetinviTest);
            var listsOwnedByTweetinviTest = await _tweetinviClient.Lists.GetListsOwnedByUserIterator(EndToEndTestConfig.TweetinviTest).NextPageAsync();

            await Task.Delay(_twitterEventualConsistencyDelay);
            await listFirstCreatedAsPublic.UpdateAsync(new ListMetadataParameters
            {
                PrivacyMode = PrivacyMode.Private
            });

            // cleanup
            await Task.Delay(_twitterEventualConsistencyDelay);

            await retrievedPrivateList.DestroyAsync();
            await _tweetinviTestClient.Lists.DestroyListAsync(listFirstCreatedAsPublic);

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
        public async Task MembersAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _tweetinviTestClient.Lists.CreateListAsync("members-test-list", PrivacyMode.Public);
            await Task.Delay(_twitterEventualConsistencyDelay);

            await publicList.AddMemberAsync(EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterAdding = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfListAsync(publicList, EndToEndTestConfig.TweetinviApi);
            await _tweetinviTestClient.Lists.AddMemberToListAsync(publicList, EndToEndTestConfig.TweetinviTest);
            await _tweetinviTestClient.Lists.AddMembersToListAsync(publicList, new[] { "bbc", "lemondefr" });

            await Task.Delay(TimeSpan.FromSeconds(5));

            await _tweetinviClient.Lists.GetMembersOfListAsync(new GetMembersOfListParameters(publicList));
            var membersIterator = _tweetinviTestClient.Lists.GetMembersOfListIterator(new GetMembersOfListParameters(publicList)
            {
                PageSize = 2
            });

            var publicListMembers = new List<IUser>();
            while (!membersIterator.Completed)
            {
                publicListMembers.AddRange(await membersIterator.NextPageAsync());
            }

            await _tweetinviClient.Lists.GetUserListMembershipsAsync(EndToEndTestConfig.TweetinviTest);
            var listsTweetinviTestIsMemberOfIterator = _tweetinviClient.Lists.GetUserListMembershipsIterator(EndToEndTestConfig.TweetinviTest);
            var listsTweetinviTestIsMemberOf = (await listsTweetinviTestIsMemberOfIterator.NextPageAsync()).ToArray();

            await _tweetinviTestClient.Lists.RemoveMemberFromListAsync(publicList, EndToEndTestConfig.TweetinviApi);
            var isTweetinviApiAMemberAfterRemoving = await _tweetinviTestClient.Lists.CheckIfUserIsMemberOfListAsync(publicList, EndToEndTestConfig.TweetinviApi);

            await _tweetinviTestClient.Lists.RemoveMembersFromListAsync(publicList, new[] { "bbc", "lemondefr" });
            await Task.Delay(_twitterEventualConsistencyDelay);
            var updatedList = await _tweetinviTestClient.Lists.GetListAsync(publicList);

            await publicList.DestroyAsync();

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
        public async Task SubscribersAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _tweetinviTestClient.Lists.CreateListAsync("subscribers-test-list", PrivacyMode.Public);
            await Task.Delay(_twitterEventualConsistencyDelay);

            await _tweetinviClient.Lists.SubscribeToListAsync(publicList);
            await _protectedClient.Lists.SubscribeToListAsync(publicList);
            await _tweetinviTestClient.Lists.SubscribeToListAsync(publicList);

            await _tweetinviClient.Lists.GetListSubscribersAsync(publicList);
            var subscriberIterator = _tweetinviTestClient.Lists.GetListSubscribersIterator(new GetListSubscribersParameters(publicList)
            {
                PageSize = 2
            });

            var subscribers = new List<IUser>();
            while (!subscriberIterator.Completed)
            {
                subscribers.AddRange(await subscriberIterator.NextPageAsync());
            }

            await _tweetinviClient.Lists.GetUserListSubscriptionsAsync(EndToEndTestConfig.TweetinviApi);
            var subscriptionsIterator = _tweetinviTestClient.Lists.GetUserListSubscriptionsIterator(EndToEndTestConfig.TweetinviApi);
            var subscriptions = (await subscriptionsIterator.NextPageAsync()).ToArray();

            var bbcSubscriber = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfListAsync(publicList, "bbc");
            var tweetinviSubscriberBeforeRemove = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfListAsync(publicList, EndToEndTestConfig.TweetinviApi);

            await _tweetinviClient.Lists.UnsubscribeFromListAsync(publicList);

            await Task.Delay(TimeSpan.FromSeconds(5));

            var tweetinviSubscriberAfterRemove = await _tweetinviTestClient.Lists.CheckIfUserIsSubscriberOfListAsync(publicList, EndToEndTestConfig.TweetinviApi);

            await publicList.DestroyAsync();

            // assert
            Assert.Equal(subscribers.Count, 3);
            Assert.False(bbcSubscriber);
            Assert.True(tweetinviSubscriberBeforeRemove);
            Assert.False(tweetinviSubscriberAfterRemove);
            Assert.Contains(subscriptions, list => list.Id == publicList.Id);
        }

        [Fact]
        public async Task TweetsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var publicList = await _protectedClient.Lists.CreateListAsync("members-test-list", PrivacyMode.Public);

            var tweet = await _tweetinviTestClient.Tweets.PublishTweetAsync("Testing that members are working" + Guid.NewGuid());

            await publicList.AddMemberAsync("tweetinvitest");
            await Task.Delay(_twitterEventualConsistencyDelay); // give some time to twitter for timeline generation

            // act
            var getTweetsParameters = new GetTweetsFromListParameters(publicList)
            {
                PageSize = 2
            };

            await _protectedClient.Lists.GetTweetsFromListAsync(getTweetsParameters);
            var tweetsIterator = _protectedClient.Lists.GetTweetsFromListIterator(getTweetsParameters);
            var listTweetsPage1 = await tweetsIterator.NextPageAsync();
            getTweetsParameters.PageSize = 4;
            var listTweetsPage2 = await tweetsIterator.NextPageAsync();

            await tweet.DestroyAsync();
            await publicList.DestroyAsync();

            // assert
            Assert.Contains(listTweetsPage1, listTweet => listTweet.Id == tweet.Id);
            Assert.Equal(listTweetsPage1.Count(), 2);
            Assert.DoesNotContain(listTweetsPage2, listTweet => listTweet.Id == tweet.Id);
            Assert.Equal(listTweetsPage2.Count(), 4);
        }
    }
}