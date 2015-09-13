using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Examplinvi.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static string CONSUMER_KEY = "PUT_YOURS!";
        public static string CONSUMER_SECRET = "PUT_YOURS!";

        public ActionResult TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(CONSUMER_KEY, CONSUMER_SECRET);
            var redirectURL = "http://" + Request.Url.Authority + "/Home/ValidateTwitterAuth";
            var url = CredentialsCreator.GetAuthorizationURL(appCreds, redirectURL);

            return new RedirectResult(url);
        }

        public ActionResult ValidateTwitterAuth()
        {
            var verifierCode = Request.Params.Get("oauth_verifier");
            var authorizationId = Request.Params.Get("authorization_id");

            var userCreds = CredentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authorizationId);
            var user = Tweetinvi.User.GetLoggedUser(userCreds);

            ViewBag.User = user;

            return View();
        }
    }
}