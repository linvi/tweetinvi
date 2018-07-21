using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class MessageAsync
    {
        // Factory
        public static async Task<IMessage> GetExistingMessage(long messageId)
        {
            return await Sync.ExecuteTaskAsync(() => Message.GetExistingMessage(messageId));
        }

        // Controller
        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static async Task<IResultsWithCursor<IMessage>> GetLatestMessages(
            int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            string cursor = null;
            IEnumerable<IMessage> messages =
                await Sync.ExecuteTaskAsync(() => Message.GetLatestMessages(count, out cursor));

            return generateResultsWithCursor(messages, cursor);
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static async Task<IResultsWithCursor<IMessage>> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            string cursor = null;
            IEnumerable<IMessage> messages =
                await Sync.ExecuteTaskAsync(() => Message.GetLatestMessages(queryParameters, out cursor));

            return generateResultsWithCursor(messages, cursor);
        }

        private static IResultsWithCursor<IMessage> generateResultsWithCursor(IEnumerable<IMessage> messages,
            string cursor)
        {
            if (messages == null)
            {
                return null;
            }

            IResultsWithCursor<IMessage> resultsWithCursor = TweetinviContainer.Resolve<IResultsWithCursor<IMessage>>();
            resultsWithCursor.Results = messages;
            resultsWithCursor.Cursor = cursor;
            return resultsWithCursor;
        }

        // Publish Message
        public static async Task<IMessage> PublishMessage(IPublishMessageParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(parameters));
        }

        public static async Task<IMessage> PublishMessage(string text, long targetUserId)
        {
            return await Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUserId));
        }

        // Destroy Message
        public static async Task<bool> DestroyMessage(IMessage message)
        {
            return await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(message));
        }

        public static async Task<bool> DestroyMessage(IEventDTO eventDTO)
        {
            return  await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(eventDTO));
        }

        public static async Task<bool> DestroyMessage(long messageId)
        {
            return  await Sync.ExecuteTaskAsync(() => Message.DestroyMessage(messageId));
        }
    }
}
