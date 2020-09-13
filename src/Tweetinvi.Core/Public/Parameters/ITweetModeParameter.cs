namespace Tweetinvi.Parameters
{
    public interface ITweetModeParameter
    {
        /// <summary>
        /// Decide whether to use Extended or Compat mode
        /// </summary>
        TweetMode? TweetMode { get; set; }
    }
}