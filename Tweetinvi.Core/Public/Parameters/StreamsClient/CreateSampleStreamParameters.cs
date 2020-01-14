namespace Tweetinvi.Parameters
{
    public interface ICreateSampleStreamParameters : ICreateTweetStreamParameters
    {
    }

    public class CreateSampleStreamParameters : CreateTweetStreamParameters, ICreateSampleStreamParameters
    {
    }
}