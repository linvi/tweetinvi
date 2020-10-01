namespace Tweetinvi.Parameters.V2
{
    public interface IChangeTweetReplyVisibilityParameters : ICustomRequestParameters
    {
        long Id { get; set; }
        TweetReplyVisibility Visibility { get; set; }
    }

    public enum TweetReplyVisibility
    {
        Hidden = 0,
        Visible = 1
    }

    public class ChangeTweetReplyVisibilityParameters : CustomRequestParameters, IChangeTweetReplyVisibilityParameters
    {
        public ChangeTweetReplyVisibilityParameters(long tweetId, TweetReplyVisibility visibility)
        {
            Id = tweetId;
            Visibility = visibility;
        }

        public long Id { get; set; }
        public TweetReplyVisibility Visibility { get; set; }
    }
}