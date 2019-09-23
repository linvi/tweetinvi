using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetHelper
    {
        long GetOldestTweetId(IEnumerable<ITweetDTO> tweetDTOs);
    }

    public class TweetHelper : ITweetHelper
    {
        public long GetOldestTweetId(IEnumerable<ITweetDTO> tweetDTOs)
        {
            return long.Parse(tweetDTOs.Min(x => x.IdStr));
        }
    }
}