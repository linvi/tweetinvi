using System.Diagnostics;
using System.Threading.Tasks;
using Examplinvi.ASP.NET.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Parameters.Auth;
using Tweetinvi.Auth;

namespace Examplinvi.ASP.NET.Core.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// NOTE PLEASE CHANGE THE IMPLEMENTATION OF IAuthenticationTokenProvider to match your needs
        /// </summary>
        private static readonly IAuthenticationTokenProvider _myAuthProvider = new AuthenticationTokenProvider();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private static ITwitterClient GetAppClient()
        {
            var appCreds = MyCredentials.GenerateAppCreds();
            return new TwitterClient(appCreds);
        }

        public async Task<ActionResult> TwitterAuth()
        {
            var appClient = GetAppClient();
            var redirectURL = "https://" + Request.Host.Value + "/Home/ValidateTwitterAuth";
            var authenticationContext = await appClient.Auth.RequestAuthenticationUrl(redirectURL, _myAuthProvider);

            return new RedirectResult(authenticationContext.AuthorizationURL);
        }

        public async Task<ActionResult> ValidateTwitterAuth()
        {
            var appClient = GetAppClient();

            var requestParameters = await RequestCredentialsParameters.FromCallbackUrl(Request.QueryString.Value, _myAuthProvider);
            var userCreds = await appClient.Auth.RequestCredentials(requestParameters);

            var userClient = new TwitterClient(userCreds);
            var user = await userClient.Account.GetAuthenticatedUser();

            ViewBag.User = user;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}