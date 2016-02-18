namespace Tweetinvi.Core.Parameters
{
    public interface IMessagesRetrieveRequestParametersBase : ICustomRequestParameters
    {
        /// <summary>
        /// Maximum number of messages to retrieve.
        /// </summary>
        int MaximumNumberOfMessagesToRetrieve { get; set; }

        /// <summary>
        /// Returns tweets with an ID greater than the specified value.
        /// </summary>
        long? SinceId { get; set; }

        /// <summary>
        /// Returns tweets with an ID lower than the specified value.
        /// </summary>
        long? MaxId { get; set; }

        /// <summary>
        /// Include tweet entities.
        /// </summary>
        bool IncludeEntities { get; set; }

        /// <summary>
        /// Messages with more than 140 characters will be truncated
        /// if this value is set to false.
        /// </summary>
        bool FullText { get; set; }
    }
}