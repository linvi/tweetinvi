using System.Collections.Generic;
using Tweetinvi.AspNet.Modules;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.AspNet.Public
{
    public class WebhooksPlugin : ITweetinviModule
    {
        public static ITweetinviContainer Container { get; private set; }
        private readonly List<ITweetinviModule> _moduleCatalog;

        public WebhooksPlugin()
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
