using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class TrendsAsync
    {
        public static async Task<IPlaceTrends> GetTrendsAt(long woeid)
        {
            return await Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeid));
        }

        public static async Task<IPlaceTrends> GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return await Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeIdLocation));
        }
    }
}
