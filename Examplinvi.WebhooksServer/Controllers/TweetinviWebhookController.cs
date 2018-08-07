using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examplinvi.WebhooksServer;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;

namespace WebApplication1.Controllers
{
    [Route("tweetinvi")]
    public class TweetinviWebhookController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<bool> ChallengeWebhook(string environment, string webhookId)
        {
            Auth.SetApplicationOnlyCredentials(Startup.TweetinviWebhookConfiguration.ConsumerOnlyCredentials);
            return await Webhooks.ChallengeWebhookAsync(environment, webhookId);
        }

        [HttpPost]
        public async Task<bool> RegisterWebhook(string environment, string url, string userId)
        {
            var credentials = await GetUserCredentials(userId);
            var result = await Webhooks.RegisterWebhookAsync(environment, url, credentials);

            return result != null;
        }

        [HttpGet]
        public async Task<WebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(Startup.TweetinviWebhookConfiguration.ConsumerOnlyCredentials);
            return webhookEnvironments.Cast<WebhookEnvironmentDTO>().ToArray();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<ITwitterCredentials> GetUserCredentials(string userId)
        {
            // Implement your own logic for user credentials data retrieval
            return await Task.FromResult(new TwitterCredentials());
        }
    }
}
