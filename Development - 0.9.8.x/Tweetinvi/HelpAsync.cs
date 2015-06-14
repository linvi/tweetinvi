using System.Threading.Tasks;

namespace Tweetinvi
{
    public static class HelpAsync
    {
        public static async Task<string> GetTwitterPrivacyPolicy()
        {
            return await Sync.ExecuteTaskAsync(() => Help.GetTwitterPrivacyPolicy());
        }
    }
}
