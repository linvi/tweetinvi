using System;

namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IWebhookDTO
    {
        string Id { get; set; }
        string Url { get; set; }
        bool Valid { get; set; }
        DateTime CreatedAt { get; set; }
        Uri Uri { get; }
    }
}
