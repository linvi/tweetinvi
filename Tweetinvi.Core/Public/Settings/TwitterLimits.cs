namespace Tweetinvi
{
    public class TwitterLimits
    {
        public TwitterLimits()
        {
            Tweets = new TweetLimits();
        }

        public TweetLimits Tweets { get; private set; }

        public TwitterLimits Clone()
        {
            var clone = new TwitterLimits
            {
                Tweets = Tweets.Clone()
            };

            return clone;
        }
    }

    public class TweetLimits
    {
        public short GetTweetsRequestMaxSize { get; set; } = 100;
        public short GetRetweetsRequestMaxSize { get; set; } = 100;

        public TweetLimits Clone()
        {
            var clone = new TweetLimits
            {
                GetTweetsRequestMaxSize = GetTweetsRequestMaxSize,
                GetRetweetsRequestMaxSize = GetRetweetsRequestMaxSize
            };

            return clone;
        }
    }
}
