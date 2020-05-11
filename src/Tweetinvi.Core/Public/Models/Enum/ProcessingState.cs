namespace Tweetinvi.Models
{
    public enum ProcessingState
    {
        Undefined,

        /// <summary>
        /// Processing is pending
        /// </summary>
        Pending,

        /// <summary>
        /// Processing is in progress
        /// </summary>
        InProgress,

        /// <summary>
        /// Processing has completed successfully
        /// </summary>
        Succeeded,

        /// <summary>
        /// Processing has failed to complete
        /// </summary>
        Failed
    }
}
