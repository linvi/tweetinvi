namespace Tweetinvi.Parameters
{
    public interface ICreateFilteredStreamParameters : ICreateTrackedStreamParameters
    {
    }

    public class CreateFilteredStreamParameters : CreateTrackedStreamParameters, ICreateFilteredStreamParameters
    {
    }
}