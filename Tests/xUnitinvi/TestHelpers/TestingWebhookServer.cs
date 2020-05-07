using System;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;

namespace xUnitinvi.TestHelpers
{
    public class TestingWebhookServer : IDisposable
    {
        private readonly HttpListener _server;

        public TestingWebhookServer(int port)
        {
            _server = new HttpListener();
            _server.Prefixes.Add($"http://*:{port}/");
        }

        public void Start()
        {
            _server.Start();

            // This is acceptable as the server will never need to be started straight away
#pragma warning disable 4014
            RunServerAsync();
#pragma warning restore 4014
        }

        private async Task RunServerAsync()
        {
            while (_server.IsListening)
            {
                var context = await _server.GetContextAsync();
                this.Raise(OnRequest, context);
            }
        }

        public EventHandler<HttpListenerContext> OnRequest;

        public void Dispose()
        {
            ((IDisposable) _server)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}