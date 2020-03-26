namespace Tweetinvi.Parameters
{
    public interface ICreateFilteredTweetStreamParameters : ICreateTrackedTweetStreamParameters
    {
    }

    public class CreateFilteredTweetStreamParameters : CreateTrackedTweetStreamParameters, ICreateFilteredTweetStreamParameters
    {
    }
}