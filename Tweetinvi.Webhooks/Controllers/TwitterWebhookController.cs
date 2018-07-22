using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Webhooks.Controllers
{
    [Route("tweetinvi-webhooks")]
    public class TwitterWebhookController : Controller
    {
        [HttpGet]
        public string Get()
        {
            HttpContext.Response.ContentType = "application/json; charset=utf-8";

            var crcToken = HttpContext.Request.Query["crc_token"];
            var oauth = HttpContext.Items["consumer_secret"]?.ToString();

            if (crcToken.IsNullOrEmpty() || oauth.IsNullOrEmpty())
            {
                //HttpContext.Response.WriteAsync()
                return "Operation not permitted";
            }

            return "old";
        }

        
    }
}