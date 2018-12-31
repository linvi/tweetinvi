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
        public static Task<IMessage> GetExistingMessage(long messageId)
        {
            return Sync.ExecuteTaskAsync(() => Message.GetExistingMessage(messageId));
        }

        // Controller
        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static Task<IResultsWithCursor<IMessage>> GetLatestMessages(
            int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return Sync.ExecuteTaskAsync(() =>
            {
                IEnumerable<IMessage> messages = Message.GetLatestMessages(count, out string cursor);
                return generateResultsWithCursor(messages, cursor);
            });
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static Task<IResultsWithCursor<IMessage>> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            return Sync.ExecuteTaskAsync(() =>
            {
                IEnumerable<IMessage> messages = Message.GetLatestMessages(queryParameters, out string cursor);
                return generateResultsWithCursor(messages, cursor);
            });
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
        public static Task<IMessage> PublishMessage(IPublishMessageParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => Message.PublishMessage(parameters));
        }

        public static Task<IMessage> PublishMessage(string text, long targetUserId)
        {
            return Sync.ExecuteTaskAsync(() => Message.PublishMessage(text, targetUserId));
        }

        // Destroy Message
        public static Task<bool> DestroyMessage(IMessage message)
        {
            return Sync.ExecuteTaskAsync(() => Message.DestroyMessage(message));
        }

        public static Task<bool> DestroyMessage(IEventDTO eventDTO)
        {
            return  Sync.ExecuteTaskAsync(() => Message.DestroyMessage(eventDTO));
        }

        public static Task<bool> DestroyMessage(long messageId)
        {
            return  Sync.ExecuteTaskAsync(() => Message.DestroyMessage(messageId));
        }
    }
}
