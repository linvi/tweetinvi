using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Modules;

namespace Tweetinvi
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
            _moduleCatalog.Add(new WebhooksSharedModule());

            _moduleCatalog.ForEach(module =>
            {
                module.Initialize(container);
            });
        }
    }
}
