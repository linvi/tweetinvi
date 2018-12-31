using System.Threading.Tasks;

namespace Tweetinvi
{
    public static class HelpAsync
    {
        /// <summary>
        /// Returns the privacy statement of Twitter.
        /// </summary>
        public static Task<string> GetTwitterPrivacyPolicy()
        {
            return Sync.ExecuteTaskAsync(Help.GetTwitterPrivacyPolicy);
        }
    }
}