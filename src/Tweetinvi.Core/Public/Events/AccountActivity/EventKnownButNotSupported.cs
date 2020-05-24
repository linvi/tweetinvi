namespace Tweetinvi.Events
{
    /// <summary>
    /// The json message was understood, but Tweetinvi could not properly analyze it
    /// as it the reason why it was raised are not yet supported
    /// </summary>
    public class EventKnownButNotSupported
    {
        public EventKnownButNotSupported(string json, BaseAccountActivityEventArgs accountActivityEventArgs)
        {
            Json = json;
            EventArgs = accountActivityEventArgs;
        }

        /// <summary>
        /// The json object that Tweetinvi could not fully understand
        /// </summary>
        public string Json { get; }

        public BaseAccountActivityEventArgs EventArgs { get; }
    }
}
