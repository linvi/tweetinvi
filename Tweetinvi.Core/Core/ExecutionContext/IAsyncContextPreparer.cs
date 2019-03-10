namespace Tweetinvi.Core.ExecutionContext
{
    public interface IAsyncContextPreparer
    {
        /// <summary>
        /// PrepareFromParentAsyncContext the Execution Context for copying.
        /// Any objects in the Execution Context whose (modified) values we want to be available outside of the copied
        /// context need to be instantiated before copying.
        /// </summary>
        void PrepareFromParentAsyncContext();

        void PrepareFromChildAsyncContext();
    }
}
