using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class MessagesEndToEndTests : TweetinviTest
    {
        public MessagesEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task Messages_CRUD()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var messageTextIdentifier = $"hello from tweetinvi {Guid.NewGuid()}";
            var message = await _tweetinviClient.Messages.PublishMessage(messageTextIdentifier, EndToEndTestConfig.TweetinviTest.UserId);
            var publishedMessage = await _tweetinviClient.Messages.GetMessage(message.Id);
            var responseMessage = await _tweetinviTestClient.Messages.PublishMessage($"you are soo nice {Guid.NewGuid()}", EndToEndTestConfig.TweetinviApi.UserId);

            // wait for twitter eventual consistency
            await Task.Delay(TimeSpan.FromSeconds(70));

            try
            {
                await _tweetinviClient.Messages.GetMessages();
                var messagesIterator = _tweetinviClient.Messages.GetMessagesIterator(new GetMessagesParameters
                {
                    PageSize = 1
                });

                var messages = new List<IMessage>();
                var totalCursorRequestRun = 0;

                // < 2 to handle empty results from twitter
                // note - `messagesIterator.Completed` is never true as Twitter always provide cursor
                while (messages.Count < 2 && !messagesIterator.Completed && totalCursorRequestRun < 5)
                {
                    ++totalCursorRequestRun; // prevent getting out of RateLimits when Twitter continuously returns no elements
                    var page = (await messagesIterator.MoveToNextPage()).ToArray();
                    _logger.WriteLine($"Received {page.Length} elements");
                    messages.AddRange(page);
                }

                Assert.Equal(messages[0].Id, responseMessage.Id);
                Assert.Equal(messages[1].Id, publishedMessage.Id);
            }
            catch (TwitterException e)
            {
                _logger.WriteLine(e.ToString());
                throw;
            }
            finally
            {
                // messages have to be destroyed by both the sender and receiver
                // for it to no longer exists.
                await _tweetinviClient.Messages.DestroyMessage(message);
                await _tweetinviTestClient.Messages.DestroyMessage(message);
                await _tweetinviClient.Messages.DestroyMessage(responseMessage);
                await _tweetinviTestClient.Messages.DestroyMessage(responseMessage);
            }

            try
            {
                await _tweetinviClient.Messages.GetMessage(message.Id);
                throw new Exception("Should not be able to retrieve the message");
            }
            catch (TwitterException)
            {
            }

            Assert.Equal(message.Text, messageTextIdentifier);
            Assert.Equal(publishedMessage.Text, messageTextIdentifier);
        }

        [Fact]
        public async Task PublishMedia()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var media = await _tweetinviClient.Upload.UploadBinary(tweetinviLogoBinary);
            var message = await _tweetinviClient.Messages.PublishMessage(new PublishMessageParameters("hello", EndToEndTestConfig.TweetinviTest.UserId)
            {
                MediaId = media.Id
            });

            await _tweetinviClient.Messages.DestroyMessage(message);
            await _tweetinviTestClient.Messages.DestroyMessage(message);

            Assert.Equal(message.AttachedMedia.Id, media.Id);
        }

        [Fact]
        public async Task PublishOptions()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var media = await _tweetinviClient.Upload.UploadBinary(tweetinviLogoBinary);
            var message = await _tweetinviClient.Messages.PublishMessage(new PublishMessageParameters("hello", EndToEndTestConfig.TweetinviTest.UserId)
            {
                QuickReplyOptions = new IQuickReplyOption[]
                {
                    new QuickReplyOption
                    {
                        Label = "Superb"
                    },
                    new QuickReplyOption
                    {
                        Label = "Cool"
                    },
                    new QuickReplyOption
                    {
                        Label = "Hum"
                    },
                }
            });

            await _tweetinviClient.Messages.DestroyMessage(message);
            await _tweetinviTestClient.Messages.DestroyMessage(message);

            Assert.Equal(message.QuickReplyOptions.Length, 3);
        }
    }
}