namespace Tweetinvi.Core.ExecutionContext
{
    public interface IAsyncContextPreparable
    {
        /// <summary>
        /// PrepareAsyncContext the current execution context to be copied.
        /// If anything within the implementing class must exist in the parent context (so that the pointer to the object
        /// on the heap is cross execution context), they should be set in this method.
        /// </summary>
        void InitializeAsyncContext();
    }
}
