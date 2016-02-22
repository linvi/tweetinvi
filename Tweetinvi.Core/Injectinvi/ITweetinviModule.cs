namespace Tweetinvi.Core.Injectinvi
{
    /// <summary>
    /// Module used to initialize Tweetinvi dependency injection
    /// </summary>
    public interface ITweetinviModule
    {
        /// <summary>
        /// Initialize the module registration.
        /// </summary>
        void Initialize();
    }
}