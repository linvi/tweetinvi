namespace Tweetinvi.Core.Enum
{
    /// <summary>
    /// Enumeration listing how the Stream is supposed to behave
    /// </summary>
    public enum StreamState
    {
        /// <summary>
        /// The stream is not running.
        /// In this state the stream configuration can be changed.
        /// </summary>
        Stop = 0,

        /// <summary>
        /// Stream is Running.
        /// </summary>
        Running = 1,

        /// <summary>
        /// Stream is paused.
        /// The stream configuration cannot be changed in this state.
        /// </summary>
        Pause = 2
    }
}