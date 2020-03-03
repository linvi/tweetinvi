namespace Tweetinvi.Models
{
    public interface IQuickReplyOption
    {
        /// <summary>
        /// The text label displayed on the button face. Label text is returned as the user's message response.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Description text displayed under label text.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Metadata that will be sent back in the webhook request.
        /// </summary>
        string Metadata { get; }
    }
}
