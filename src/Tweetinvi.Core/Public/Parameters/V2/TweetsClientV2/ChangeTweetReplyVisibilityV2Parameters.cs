namespace Tweetinvi.Parameters.V2
{
    public interface IChangeTweetReplyVisibilityV2Parameters : ICustomRequestParameters
    {
        string Id { get; set; }
        TweetReplyVisibility Visibility { get; set; }
    }

    public enum TweetReplyVisibility
    {
        Hidden = 0,
        Visible = 1
    }

    public class ChangeTweetReplyVisibilityV2Parameters : CustomRequestParameters, IChangeTweetReplyVisibilityV2Parameters
    {
        public ChangeTweetReplyVisibilityV2Parameters(long tweetId, TweetReplyVisibility visibility)
        {
            Id = tweetId.ToString();
            Visibility = visibility;
        }

        public ChangeTweetReplyVisibilityV2Parameters(string tweetId, TweetReplyVisibility visibility)
        {
            Id = tweetId;
            Visibility = visibility;
        }

        public string Id { get; set; }
        public TweetReplyVisibility Visibility { get; set; }
    }
}