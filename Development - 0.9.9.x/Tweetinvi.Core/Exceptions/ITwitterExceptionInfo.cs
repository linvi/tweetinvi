namespace Tweetinvi.Core.Exceptions
{
    public interface ITwitterExceptionInfo
    {
        string Message { get; set; }
        int Code { get; set; }
        string Label { get; set; }
    }
}