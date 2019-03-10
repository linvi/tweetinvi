using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Core.ExecutionContext
{
    public class AsyncContextPreparer : IAsyncContextPreparer
    {
        private readonly IAsyncContextPreparable[] _asyncContextPreparables;

        public AsyncContextPreparer(ITweetinviContainer container)
        {
            // This logic and casting are necessary to ensure that Autofac does not create multiple instances of objects
            _asyncContextPreparables = new[] {
                (IAsyncContextPreparable)container.Resolve<ITweetinviSettingsAccessor>(),
                (IAsyncContextPreparable)container.Resolve<ICredentialsAccessor>()
            };
        }

        public void PrepareFromParentAsyncContext()
        {
            foreach (var asyncContextPreparable in _asyncContextPreparables)
            {
                asyncContextPreparable.InitializeFromParentAsyncContext();
            }
        }

        public void PrepareFromChildAsyncContext()
        {
            foreach (var asyncContextPreparable in _asyncContextPreparables)
            {
                asyncContextPreparable.InitializeFromChildAsyncContext();
            }
        }
    }
}
