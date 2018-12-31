using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class TrendsAsync
    {
        public static Task<IPlaceTrends> GetTrendsAt(long woeid)
        {
            return Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeid));
        }

        public static Task<IPlaceTrends> GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeIdLocation));
        }
    }
}
