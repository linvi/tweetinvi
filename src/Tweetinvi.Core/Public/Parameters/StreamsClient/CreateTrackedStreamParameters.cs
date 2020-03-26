namespace Tweetinvi.Parameters
{
    public interface ICreateTrackedTweetStreamParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Decide whether to use Extended or Compat mode
        /// </summary>
        TweetMode? TweetMode { get; set; }
    }

    public class CreateTrackedTweetStreamParameters : CustomRequestParameters, ICreateTrackedTweetStreamParameters
    {
        public TweetMode? TweetMode { get; set; }
    }
}