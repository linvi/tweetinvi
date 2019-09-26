using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Testinvi.IntegrationTests
{
    public class TwitterLists
    {
        public async Task TwitterList_Lifecycle()
        {
            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var client = new TwitterClient(creds);

            var authenticatedUser = await client.Account.GetAuthenticatedUser();
            var newList = await TwitterList.CreateList("myTemporaryList", PrivacyMode.Private, "tmp");
            var userLists = await TwitterList.GetUserSubscribedLists(authenticatedUser);
            var newListVerify = await TwitterList.GetExistingList(newList);
            var updateParameter = new TwitterListUpdateParameters { Name = "piloupe" };
            await newListVerify.Update(updateParameter);
            await newListVerify.Destroy();
        }
    }
}