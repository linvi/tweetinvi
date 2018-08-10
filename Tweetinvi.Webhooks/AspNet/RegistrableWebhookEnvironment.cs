using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.AspNet
{
    public interface IRegistrableWebhookEnvironment
    {
        string Name { get; set; }
        IWebhookDTO[] Webhooks { get; }
        IConsumerCredentials Credentials { get; set; }

        void AddWebhook(IWebhookDTO webhook);
        void RemoveWebhook(IWebhookDTO webhook);
    }

    public class RegistrableWebhookEnvironment : IRegistrableWebhookEnvironment
    {
        private List<IWebhookDTO> _webhooks;

        public RegistrableWebhookEnvironment()
        {
            _webhooks = new List<IWebhookDTO>();
        }

        public RegistrableWebhookEnvironment(IWebhookEnvironmentDTO environment)
        {
            Name = environment.Name;
            _webhooks = environment.Webhooks?.ToList() ?? new List<IWebhookDTO>();
        }

        public string Name { get; set; }

        public IWebhookDTO[] Webhooks
        {
            get { return _webhooks.ToArray(); }
        }

        public IConsumerCredentials Credentials { get; set; }

        public void AddWebhook(IWebhookDTO webhook)
        {
            if (_webhooks.Contains(webhook))
            {
                return;
            }

            _webhooks.Add(webhook);
        }

        public void RemoveWebhook(IWebhookDTO webhook)
        {
            _webhooks.Remove(webhook);
        }
    }
}
