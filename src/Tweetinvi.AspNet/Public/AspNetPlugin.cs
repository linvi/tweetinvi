using System.Collections.Generic;
using Tweetinvi.AspNet.Modules;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.AspNet
{
    public class AspNetPlugin : ITweetinviModule
    {
        public static ITweetinviContainer Container { get; private set; }
        private readonly List<ITweetinviModule> _moduleCatalog;

        public AspNetPlugin()
        {
            _moduleCatalog = new List<ITweetinviModule>();
        }

        public void Initialize(ITweetinviContainer container)
        {
            Container = container;

            InitializeModules(container);
        }

        private void InitializeModules(ITweetinviContainer container)
        {
            _moduleCatalog.Add(new TweetinviAspNetModule());

            _moduleCatalog.ForEach(module =>
            {
                module.Initialize(container);
            });
        }
    }
}
