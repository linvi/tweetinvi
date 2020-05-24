using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class AccountActivityStreamEndToEndTests : TweetinviTest
    {
        public AccountActivityStreamEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        // NOTE : Many Task.Delay can be found in the following test.
        // The reason is that Twitter requires quite a lot of time to propagate the event to the webhooks.
        // Without the delays between all the operations this would result in events not being received.
        // Even with the currently high delay, there are still some errors happening due to time issues

        [Fact]
        public async Task EventsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAccountActivityStreamTests)
                return;

            var environment = "sandbox";
            var timeoutBetweenOperations = TimeSpan.FromSeconds(25);

            await AccountActivityEndToEndTests.RunAccountActivityTestAsync(async config =>
            {
                // arrange
                var client = config.AccountActivityClient;
                var accountActivityHandler = config.AccountActivityRequestHandler;

                var webhookUrl = $"{await config.Ngrok.GetUrlAsync()}?accountActivityEnvironment={environment}";
                await client.AccountActivity.CreateAccountActivityWebhookAsync(environment, webhookUrl);

                // act
                var userClient = new TwitterClient(EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.Credentials);

                var user = await userClient.Users.GetAuthenticatedUserAsync();
                var stream = accountActivityHandler.GetAccountActivityStream(user.Id, environment);

                var state = new AccountActivtyEventsState();

                RegisterEvents(stream, state);

                await userClient.AccountActivity.SubscribeToAccountActivityAsync(environment);
                await Task.Delay(TimeSpan.FromSeconds(30)); // long timeout as twitter does not start the webhook straight away

                var tweet = await userClient.Tweets.PublishTweetAsync($"testing webhooks -> v1");
                await Task.Delay(timeoutBetweenOperations);
                await userClient.Tweets.FavoriteTweetAsync(tweet);
                await Task.Delay(timeoutBetweenOperations);

                await tweet.DestroyAsync();
                await Task.Delay(timeoutBetweenOperations);

                await userClient.Users.FollowUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);
                await userClient.Users.UnfollowUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);

                await userClient.Users.MuteUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);
                await userClient.Users.UnmuteUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);

                await userClient.Users.BlockUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);
                await userClient.Users.UnblockUserAsync(EndToEndTestConfig.TweetinviTest);
                await Task.Delay(timeoutBetweenOperations);

                var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
                var media = await userClient.Upload.UploadBinaryAsync(tweetinviLogoBinary);
                await userClient.Messages.PublishMessageAsync(new PublishMessageParameters("hello from tweetinvi -> https://github.com/linvi/tweetinvi", EndToEndTestConfig.TweetinviTest.UserId)
                {
                    MediaId = media.Id
                });

                await Task.Delay(timeoutBetweenOperations);
                await _tweetinviTestClient.Messages.PublishMessageAsync("Nice to hear from your!", EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId);
                await Task.Delay(timeoutBetweenOperations);

                // TODO - Require more test for messages... - was not possible to do as messages were not yet implemented

                var stateBeforeUnsubscribe = state.Clone();

                // we are now making sure that we have unsubscribed
                await client.AccountActivity.UnsubscribeFromAccountActivityAsync(environment, EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId);
                await Task.Delay(10000); // long timeout as twitter does not start the webhook straight away

                var tweet2 = await userClient.Tweets.PublishTweetAsync($"testing webhooks -> v2");
                await Task.Delay(timeoutBetweenOperations);
                await tweet2.DestroyAsync();
                await Task.Delay(timeoutBetweenOperations);

                // assert - cleanup
                await AccountActivityEndToEndTests.CleanAllEnvironmentsAsync(client);

                stateBeforeUnsubscribe.EventsReceived.ForEach(eventReceived => { _logger.WriteLine(eventReceived); });

                Assert.Equal(state.TweetCreated.Count, 1);
                Assert.Equal(state.TweetDeleted.Count, 1);
                Assert.Equal(state.TweetFavorited.Count, 1);

                Assert.Equal(state.UserFollowed.Count, 1);
                Assert.Equal(state.UserUnfollowed.Count, 1);

                Assert.Equal(state.UserBlocked.Count, 1);
                Assert.Equal(state.UserUnblocked.Count, 1);

                Assert.Equal(state.UserMuted.Count, 1);
                Assert.Equal(state.UserUnmuted.Count, 1);

                Assert.Equal(state.MessageSent.Count, 1);
                Assert.Equal(state.MessageReceived.Count, 1);

                Assert.Equal(state.UserReadMessage.Count, 0); // TODO CHANGE TO 1 when messages supported in 6.0
                Assert.Equal(state.UserTypingMessage.Count, 0); // TODO CHANGE TO 1 when messages supported in 6.0

                Assert.Equal(state.UnexpectedException.Count, 0);

                Assert.Equal(stateBeforeUnsubscribe.EventsReceived.Count, 11);

                Assert.Equal(state.EventsReceived.Count, stateBeforeUnsubscribe.EventsReceived.Count);
            }, _tweetinviClient, _logger);
        }

        private static void RegisterEvents(IAccountActivityStream stream, AccountActivtyEventsState state)
        {
            stream.EventReceived += (sender, args) => { state.EventsReceived.Add(args.Json); };

            stream.TweetCreated += (sender, args) => { state.TweetCreated.Add(args); };
            stream.TweetDeleted += (sender, args) => { state.TweetDeleted.Add(args); };
            stream.TweetFavorited += (sender, args) => { state.TweetFavorited.Add(args); };

            stream.UserFollowed += (sender, args) => { state.UserFollowed.Add(args); };
            stream.UserUnfollowed += (sender, args) => { state.UserUnfollowed.Add(args); };

            stream.UserBlocked += (sender, args) => { state.UserBlocked.Add(args); };
            stream.UserUnblocked += (sender, args) => { state.UserUnblocked.Add(args); };

            stream.UserMuted += (sender, args) => { state.UserMuted.Add(args); };
            stream.UserUnmuted += (sender, args) => { state.UserUnmuted.Add(args); };

            stream.MessageReceived += (sender, args) => { state.MessageReceived.Add(args); };
            stream.MessageSent += (sender, args) => { state.MessageSent.Add(args); };

            stream.UserIsTypingMessage += (sender, args) => { state.UserTypingMessage.Add(args); };
            stream.UserReadMessageConversation += (sender, args) => { state.UserReadMessage.Add(args); };

            stream.UnexpectedExceptionThrown += (sender, args) => { state.UnexpectedException.Add(args); };
        }

        class AccountActivtyEventsState
        {
            public List<string> EventsReceived { get; set; } = new List<string>();
            public List<TweetCreatedEvent> TweetCreated { get; set; } = new List<TweetCreatedEvent>();
            public List<TweetDeletedEvent> TweetDeleted { get; set; } = new List<TweetDeletedEvent>();
            public List<TweetFavoritedEvent> TweetFavorited { get; set; } = new List<TweetFavoritedEvent>();

            public List<UserFollowedEvent> UserFollowed { get; } = new List<UserFollowedEvent>();
            public List<UserUnfollowedEvent> UserUnfollowed { get; set; } = new List<UserUnfollowedEvent>();
            public List<UserBlockedEvent> UserBlocked { get; set; } = new List<UserBlockedEvent>();
            public List<UserUnblockedEvent> UserUnblocked { get; set; } = new List<UserUnblockedEvent>();
            public List<UserMutedEvent> UserMuted { get; set; } = new List<UserMutedEvent>();
            public List<UserUnmutedEvent> UserUnmuted { get; set; } = new List<UserUnmutedEvent>();

            public List<MessageReceivedEvent> MessageReceived { get; set; } = new List<MessageReceivedEvent>();
            public List<MessageSentEvent> MessageSent { get; set; } = new List<MessageSentEvent>();
            public List<UserIsTypingMessageEvent> UserTypingMessage { get; set; } = new List<UserIsTypingMessageEvent>();
            public List<UserReadMessageConversationEvent> UserReadMessage { get; set; } = new List<UserReadMessageConversationEvent>();
            public List<UnexpectedExceptionThrownEvent> UnexpectedException { get; set; } = new List<UnexpectedExceptionThrownEvent>();

            public AccountActivtyEventsState Clone()
            {
                return new AccountActivtyEventsState
                {
                    EventsReceived = new List<string>(EventsReceived)
                };
            }
        }
    }
}