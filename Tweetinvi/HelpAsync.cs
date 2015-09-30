using System.Threading.Tasks;

namespace Tweetinvi
{
    public static class HelpAsync
    {
        /// <summary>
        /// Returns the privacy statement of Twitter.
        /// </summary>
        public static async Task<string> GetTwitterPrivacyPolicy()
        {
            return await Sync.ExecuteTaskAsync(() => Help.GetTwitterPrivacyPolicy());
        }
    }
}