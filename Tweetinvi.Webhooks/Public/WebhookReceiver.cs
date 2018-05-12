using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetinvi.Webhooks.Public
{
    public interface IWebhookReceiver
    {
        string Id { get; }
        string Url { get; }

        IReadOnlyCollection<IWebhookHandler> WebhookHandlers { get; }

        Task Start();
        Task Stop();

        void AddHandler(IWebhookHandler handler);
        void RemoveHandler(IWebhookHandler handler);
    }

    public class WebhookReceiver : IWebhookReceiver
    {
        private readonly List<IWebhookHandler> _webhookHandlers;

        public WebhookReceiver()
        {
            _webhookHandlers = new List<IWebhookHandler>();
        }

        public string Id { get; }
        public string Url { get; }

        public void AddHandler(IWebhookHandler handler)
        {
            if (handler.AttachedTo != null)
            {
                throw new InvalidOperationException("A webhook handler can only be added to a single WebhookReceiver");
            }

            if (_webhookHandlers.Contains(handler))
            {
                throw new InvalidOperationException("You cannot add a Webhook multiple");
            }

            _webhookHandlers.Add(handler);

            handler.AttachedTo = this;
        }

        public void RemoveHandler(IWebhookHandler handler)
        {
            if (_webhookHandlers.Contains(handler))
            {
                _webhookHandlers.Remove(handler);
            }

            handler.AttachedTo = null;
        }

        public IReadOnlyCollection<IWebhookHandler> WebhookHandlers
        {
            get { return _webhookHandlers; }
        }

        public async Task Start()
        {
            throw new NotImplementedException();
        }

        public async Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}

