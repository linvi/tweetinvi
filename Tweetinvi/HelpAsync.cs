using System.Runtime.CompilerServices;

namespace Tweetinvi
{
    public static class HelpAsync
    {
        /// <summary>
        /// Returns the privacy statement of Twitter.
        /// </summary>
        public static ConfiguredTaskAwaitable<string> GetTwitterPrivacyPolicy()
        {
            return Sync.ExecuteTaskAsync(Help.GetTwitterPrivacyPolicy);
        }
    }
}