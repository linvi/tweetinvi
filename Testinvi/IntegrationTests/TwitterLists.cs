using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Parameters;

namespace Testinvi.IntegrationTests
{
    public class TwitterLists
    {
        public void TwitterList_Lifecycle()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var newList = TwitterList.CreateList("myTemporaryList", PrivacyMode.Private, "tmp");
            var userLists = TwitterList.GetUserSubscribedLists(authenticatedUser);
            var newListVerify = TwitterList.GetExistingList(newList);
            var updateParameter = new TwitterListUpdateParameters();
            updateParameter.Name = "piloupe";
            newListVerify.Update(updateParameter);
            newListVerify.Destroy();
        }
    }
}