using System;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.TwitterLists;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.ListsClient
{
    public class TwitterListQueryGeneratorTests
    {
        public TwitterListQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TwitterListQueryGenerator>();
        }

        private readonly FakeClassBuilder<TwitterListQueryGenerator> _fakeBuilder;

        private TwitterListQueryGenerator CreateTwitterListQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()),
                new ConstructorNamedParameter("twitterListQueryParameterGenerator", TweetinviContainer.Resolve<ITwitterListQueryParameterGenerator>()),
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()));
        }

        [Fact]
        public void GetCreateListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new CreateListParameters("list_name")
            {
                Description = "list_desc",
                PrivacyMode = PrivacyMode.Private,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetCreateListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/create.json?name=list_name&mode=private&description=list_desc&hello=world");
        }

        [Fact]
        public void GetListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/show.json?list_id=42&hello=world");
        }

        [Fact]
        public void GetListQuery_WithSlug_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListParameters(new TwitterListIdentifier("myslug", "username"))
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/show.json?slug=myslug&owner_screen_name=username&hello=world");
        }

        [Fact]
        public void GetListsSubscribedByUserQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListsSubscribedByUserParameters(42)
            {
                Reverse = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListsSubscribedByUserQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/list.json?user_id=42&reverse=true&hello=world");
        }

        [Fact]
        public void GetUpdateListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new UpdateListParameters(42)
            {
                Name = "myName",
                Description = "desc",
                PrivacyMode = PrivacyMode.Public,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUpdateListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/update.json?list_id=42&name=myName&mode=public&description=desc&hello=world");
        }

        [Fact]
        public void GetUpdateListQuery_WithSlug_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new UpdateListParameters(new TwitterListIdentifier("myslug", "username"))
            {
                Name = "myName",
                Description = "desc",
                PrivacyMode = PrivacyMode.Private,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUpdateListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/update.json?slug=myslug&owner_screen_name=username&name=myName&mode=private&description=desc&hello=world");
        }

        [Fact]
        public void GetDestroyListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new DestroyListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetDestroyListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/destroy.json?list_id=42&hello=world");
        }

        [Fact]
        public void GetDestroyListQuery_WithSlug_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new DestroyListParameters(new TwitterListIdentifier("myslug", "username"))
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetDestroyListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/destroy.json?slug=myslug&owner_screen_name=username&hello=world");
        }

        [Fact]
        public void GetListsOwnedByUserQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListsOwnedByAccountByUserParameters(42)
            {
                Cursor = "my_cursor",
                PageSize = 2,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListsOwnedByUserQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/ownerships.json?user_id=42&cursor=my_cursor&count=2&hello=world");
        }

        // MEMBERS

        [Fact]
        public void GetAddMemberToListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new AddMemberToListParameters(new TwitterListIdentifier(33), 42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetAddMemberToListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members/create.json?list_id=33&user_id=42&hello=world");
        }

        [Fact]
        public void GetMembersOfListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetMembersOfListParameters(42)
            {
                Cursor = "my_cursor",
                PageSize = 2,
                IncludeEntities = true,
                SkipStatus = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetMembersOfListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members.json?list_id=42&cursor=my_cursor&count=2&include_entities=true&skip_status=false&hello=world");
        }

        [Fact]
        public void GetAddMembersToListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new AddMembersToListParameters(42, new long?[] { 5, 6 })
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetAddMembersQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members/create_all.json?list_id=42&user_id=5%2C6&hello=world");
        }

        [Fact]
        public void GetCheckIfUserIsMemberOfListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new CheckIfUserIsMemberOfListParameters(42, 43)
            {
                IncludeEntities = true,
                SkipStatus = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetCheckIfUserIsMemberOfListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members/show.json?list_id=42&user_id=43&include_entities=true&skip_status=false&hello=world");
        }

        [Fact]
        public void GetListsUserIsMemberOfQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetUserListMembershipsParameters(42)
            {
                Cursor = "my_cursor",
                PageSize = 2,
                OnlyRetrieveAccountLists = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserListMembershipsQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/memberships.json?user_id=42&cursor=my_cursor&count=2&filter_to_owned_lists=false&hello=world");
        }

        [Fact]
        public void GetRemoveMemberFromListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new RemoveMemberFromListParameters(42, 43)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetRemoveMemberFromListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members/destroy.json?list_id=42&user_id=43&hello=world");
        }

        [Fact]
        public void GetRemoveMembersFromListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new RemoveMembersFromListParameters(42, new long?[] { 5, 6 })
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            parameters.Users.Add(new UserIdentifier("linvi"));

            // Act
            var result = queryGenerator.GetRemoveMembersFromListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/members/destroy_all.json?list_id=42&user_id=5%2C6&screen_name=linvi&hello=world");
        }

        // **************
        // SUBSCRIPTIONS
        // **************

        [Fact]
        public void GetSubscribeAccountToListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new SubscribeToListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetSubscribeToListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/subscribers/create.json?list_id=42&hello=world");
        }

        [Fact]
        public void GetListSubscribersQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListSubscribersParameters(42)
            {
                Cursor = "my_cursor",
                PageSize = 45,
                IncludeEntities = false,
                SkipStatus = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListSubscribersQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/subscribers.json?list_id=42&cursor=my_cursor&count=45&include_entities=false&skip_status=true&hello=world");
        }

        [Fact]
        public void GetCheckIfUserIsSubscriberOfListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new CheckIfUserIsSubscriberOfListParameters(42, 43)
            {
                IncludeEntities = true,
                SkipStatus = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetCheckIfUserIsSubscriberOfListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/subscribers/show.json?list_id=42&user_id=43&include_entities=true&skip_status=false&hello=world");
        }

        [Fact]
        public void GetUserListSubscriptionsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetUserListSubscriptionsParameters(42)
            {
                Cursor = "my_cursor",
                PageSize = 51,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserListSubscriptionsQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/subscriptions.json?user_id=42&cursor=my_cursor&count=51&hello=world");
        }

        [Fact]
        public void GetUnSubscribeUserFromListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new UnsubscribeFromListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUnsubscribeFromListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/subscribers/destroy.json?list_id=42&hello=world");
        }
    }
}