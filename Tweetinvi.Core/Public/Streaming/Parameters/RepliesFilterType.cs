// ReSharper disable once CheckNamespace
namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// Filter the type of replies received in the stream.
    /// https://dev.twitter.com/streaming/overview/request-parameters#replies
    /// </summary>
    public enum RepliesFilterType
    {
        /// <summary>
        /// DEFAULT : You follow User B but not C. If B replies to C you WILL receive the reply in the stream.
        /// </summary>
        AllReplies,

        /// <summary>
        /// You follow User B but not C. If B replies to C you WON'T receive the reply in the stream.
        /// (Twitter default behavior)
        /// </summary>
        RepliesToKnownUsers,
    }
}