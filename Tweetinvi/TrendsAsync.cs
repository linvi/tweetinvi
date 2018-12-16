using System.Runtime.CompilerServices;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class TrendsAsync
    {
        public static ConfiguredTaskAwaitable<IPlaceTrends> GetTrendsAt(long woeid)
        {
            return Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeid));
        }

        public static ConfiguredTaskAwaitable<IPlaceTrends> GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return Sync.ExecuteTaskAsync(() => Trends.GetTrendsAt(woeIdLocation));
        }
    }
}
