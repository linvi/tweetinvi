namespace Tweetinvi.Exceptions
{
    public class TwitterAuthAbortedException : TwitterAuthException
    {
        public TwitterAuthAbortedException() : base("Authentication did not proceed until the end")
        {
        }
    }
}