using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private static ITwitterClient AppClient
        {
            get
            {
                var userCredentials = MyCredentials.GetAppCredentials();
                var appCreds = new ConsumerOnlyCredentials(userCredentials.ConsumerKey, userCredentials.ConsumerSecret);
                return new TwitterClient(appCreds);
            }
        }

        public static Dictionary<string, string> AuthenticationRequestsInProgress { get; } = new Dictionary<string, string>();
        public const string TWITTER_AUTH_QUERY_PARAMETER = "twitter_auth_request_id";

        public async Task<ActionResult> TwitterAuth()
        {
            var authenticationRequestId = Guid.NewGuid().ToString();
            var callbackUrl = $"http://{Request.Url?.Authority}/Home/ValidateTwitterAuth?{TWITTER_AUTH_QUERY_PARAMETER}={authenticationRequestId}";
            var authenticationRequest = await AppClient.Auth.RequestAuthenticationUrl(callbackUrl);

            var jsonAuthRequest = AppClient.Json.Serialize(authenticationRequest);
            // of course you will not want to save it in your application memory as your app won't be able to scale
            AuthenticationRequestsInProgress.Add(authenticationRequestId, jsonAuthRequest);
            return new RedirectResult(authenticationRequest.AuthorizationURL);
        }

        public async Task<ActionResult> ValidateTwitterAuth()
        {
            var verifierCode = Request.Params.Get("oauth_verifier");
            var authenticationRequestId = Request.Params.Get(TWITTER_AUTH_QUERY_PARAMETER);

            if (verifierCode == null)
            {
                return View();
            }

            if (!AuthenticationRequestsInProgress.TryGetValue(authenticationRequestId, out var jsonAuthRequest))
            {
                return View();
            }

            var authRequest = AppClient.Json.Deserialize<AuthenticationRequest>(jsonAuthRequest);
            var userCredentials = await AppClient.Auth.RequestCredentialsFromCallbackUrl(Request.Url, authRequest);
            var client = new TwitterClient(userCredentials);
            MyCredentials.LastAuthenticatedCredentials = userCredentials;
            var user = await client.Users.GetAuthenticatedUser();

            ViewBag.User = user;

            return View();
        }
    }
}