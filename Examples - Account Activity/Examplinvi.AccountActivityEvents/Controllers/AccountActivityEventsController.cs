using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivityEventsController
    {
        private readonly IAccountActivityRequestHandler _requestHandler;
        private readonly AccountActivityEventsManager _accountActivityEventsManager;

        public AccountActivityEventsController(IAccountActivityRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
            _accountActivityEventsManager = new AccountActivityEventsManager();
        }

        // EVENT
        public Task<string> SubscribeToEvents(string environment, long userId)
        {
            var activityStream = _requestHandler.GetAccountActivityStream(userId, environment);
            _accountActivityEventsManager.RegisterAccountActivityStream(activityStream);
            return Task.FromResult(
                $"User '{userId}' SUBSCRIBED!\n" +
                "Now try to create a tweet `hello tweetinvi and webhooks` with this user.\n" +
                "You will see in the logs of this app that a Tweet has been created!\n" +
                "Many other marvelous await you, pleas check the `AccountActivityEventsManager.cs`\n" +
                "NOTE: It may take 10 seconds for the subscription to become active on Twitter.");
        }

        public Task<string> UnsubscribeFromEvents(string environment, long userId)
        {
            var activityStream = _requestHandler.GetAccountActivityStream(userId, environment);
            _accountActivityEventsManager.UnregisterAccountActivityStream(activityStream);
            return Task.FromResult("user UNSUBSCRIBED :( \n" +
                                   "You will no longer see any events for this user.");
        }
    }
}