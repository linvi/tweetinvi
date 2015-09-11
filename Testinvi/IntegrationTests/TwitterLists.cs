using Tweetinvi;
using Tweetinvi.Core.Enum;

namespace Testinvi.IntegrationTests
{
    public class TwitterLists
    {
        public void TwitterList_Lifecycle()
        {
            var loggedUser = User.GetLoggedUser();
            var newList = TwitterList.CreateList("myTemporaryList", PrivacyMode.Private, "tmp");
            var userLists = TwitterList.GetUserSubscribedLists(loggedUser);
            var newListVerify = TwitterList.GetExistingList(newList);
            var updateParameter = TwitterList.CreateUpdateParameters();
            updateParameter.Name = "piloupe";
            newListVerify.Update(updateParameter);
            newListVerify.Destroy();
        }
    }
}