using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Tweetinvi.Webhooks.Controllers
{
    [Route("tweetinvi-webhooks")]
    public class TwitterWebhookController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value2", "value2" };
        }
    }
}