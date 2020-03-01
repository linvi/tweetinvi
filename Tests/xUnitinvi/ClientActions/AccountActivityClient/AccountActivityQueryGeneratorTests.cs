using System;
using Tweetinvi.Controllers;
using Tweetinvi.Parameters;
using Xunit;

namespace xUnitinvi.ClientActions.AccountActivityClient
{
    public class AccountActivityQueryGeneratorTests
    {
        private static AccountActivityQueryGenerator CreateQueryGenerator()
        {
            return new AccountActivityQueryGenerator();
        }

        [Fact]
        public void GetCreateAccountActivityWebhookQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new CreateAccountActivityWebhookParameters("the_env", "some_callback")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetCreateAccountActivityWebhookQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/webhooks.json?url=some_callback&hello=world");
        }

        [Fact]
        public void GetAccountActivityWebhookEnvironmentsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new GetAccountActivityWebhookEnvironmentsParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetAccountActivityWebhookEnvironmentsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/webhooks.json?hello=world");
        }

        [Fact]
        public void GetAccountActivityEnvironmentWebhooksQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new GetAccountActivityEnvironmentWebhooksParameters("my_env")
            {

                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetAccountActivityEnvironmentWebhooksQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/my_env/webhooks.json?hello=world");
        }

        [Fact]
        public void GetDeleteAccountActivityWebhookQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new DeleteAccountActivityWebhookParameters("the_env", "webhook_id")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetDeleteAccountActivityWebhookQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/webhooks/webhook_id.json?hello=world");
        }

        [Fact]
        public void GetTriggerAccountActivityWebhookCRCQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new TriggerAccountActivityWebhookCRCParameters("the_env", "webhook_id")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetTriggerAccountActivityWebhookCRCQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/webhooks/webhook_id.json?hello=world");
        }

        [Fact]
        public void GetSubscribeToAccountActivityQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new SubscribeToAccountActivityParameters("the_env")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetSubscribeToAccountActivityQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/subscriptions.json?hello=world");
        }

        [Fact]
        public void GetUnsubscribeToAccountActivityQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new UnsubscribeFromAccountActivityParameters("the_env", 42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetUnsubscribeToAccountActivityQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/subscriptions/42.json?hello=world");
        }

        [Fact]
        public void GetCountAccountActivitySubscriptionsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new CountAccountActivitySubscriptionsParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetCountAccountActivitySubscriptionsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/subscriptions/count.json?hello=world");
        }

        [Fact]
        public void GetIsAccountSubscribedToAccountActivityQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new IsAccountSubscribedToAccountActivityParameters("the_env")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetIsAccountSubscribedToAccountActivityQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/subscriptions.json?hello=world");
        }

        [Fact]
        public void GetAccountActivitySubscriptions_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new GetAccountActivitySubscriptionsParameters("the_env")
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetAccountActivitySubscriptionsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account_activity/all/the_env/subscriptions/list.json?hello=world");
        }
    }
}