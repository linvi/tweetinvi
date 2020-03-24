using System;
using Newtonsoft.Json;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.Models
{
    public class Webhook : IWebhook
    {
        public Webhook(IWebhookDTO dto)
        {
            WebhookDTO = dto;
        }

        [JsonIgnore]
        public IWebhookDTO WebhookDTO { get; }

        public string Id => WebhookDTO.Id;
        public string Url => WebhookDTO.Url;
        public bool Valid => WebhookDTO.Valid;
        public DateTime CreatedAt => WebhookDTO.CreatedAt;
        public Uri Uri => WebhookDTO.Uri;
    }
}