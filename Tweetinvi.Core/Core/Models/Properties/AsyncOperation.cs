namespace Tweetinvi.Core.Models.Properties
{
    public class FailedAsyncOperation<T> : AsyncOperation<T>
    {
        public FailedAsyncOperation()
        {
            Success = false;
        }
    }

    public class AsyncOperation<T>
    {
        public AsyncOperation()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public T Result { get; set; }
    }
}
