using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(MyCredentials.CONSUMER_KEY, MyCredentials.CONSUMER_SECRET);
            var redirectURL = "http://" + Request.Host.Value + "/Home/ValidateTwitterAuth";
            var authenticationContext = AuthFlow.InitAuthentication(appCreds, redirectURL);

            return new RedirectResult(authenticationContext.AuthorizationURL);
        }

        public ActionResult ValidateTwitterAuth()
        {
            if (Request.Query.ContainsKey("oauth_verifier") && Request.Query.ContainsKey("authorization_id"))
            {
                var verifierCode = Request.Query["oauth_verifier"];
                var authorizationId = Request.Query["authorization_id"];

                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);
                var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);

                ViewBag.User = user;
            }

            return View();
        }
    }
}
