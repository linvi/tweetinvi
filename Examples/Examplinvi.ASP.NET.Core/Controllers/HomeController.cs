using System.Diagnostics;
using Examplinvi.ASP.NET.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult TwitterAuth()
        {
            var myCreds = MyCredentials.GenerateCredentials();
            var appCreds = new TwitterCredentials(myCreds.ConsumerKey, myCreds.ConsumerSecret);
            var appClient = new TwitterClient(appCreds);
            var redirectURL = "https://" + Request.Host.Value + "/Home/ValidateTwitterAuth";
            var authenticationContext = appClient.Auth.StartAuthProcess(redirectURL).Result;

            return new RedirectResult(authenticationContext.AuthorizationURL);
        }

        public ActionResult ValidateTwitterAuth()
        {
            if (Request.Query.ContainsKey("oauth_verifier") && Request.Query.ContainsKey("authorization_id"))
            {
                var verifierCode = Request.Query["oauth_verifier"];
                var authorizationId = Request.Query["authorization_id"];

                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId).Result;
                var client = new TwitterClient(userCreds);
                var user = client.Account.GetAuthenticatedUser().Result;

                ViewBag.User = user;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}