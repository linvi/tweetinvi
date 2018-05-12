using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Webhooks;
using Tweetinvi.Webhooks;

namespace Examplinvi.NETStandard_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Plugins.Add<TweetinviWebhooksPlugin>();

            var server = Task.Run(() => Tweetinvi.Webhooks.Program.Main(args));

            var client = TweetinviContainer.Resolve<IWebhookProtocolProcessClient>();

            client.Start();

            server.Wait();
        }
    }
}
