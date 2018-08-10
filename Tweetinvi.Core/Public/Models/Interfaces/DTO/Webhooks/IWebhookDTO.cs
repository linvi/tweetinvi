using System;

namespace Tweetinvi.Models.DTO
{
    public interface IWebhookDTO
    {
        string Id { get; set; }
        string Url { get; set; }
        bool Valid { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
