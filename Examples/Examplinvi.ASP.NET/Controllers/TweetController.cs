using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.ASP.NET.Controllers
{
    public class TweetController : Controller
    {
        private static Dictionary<long, ITweet> PostedTweets = new Dictionary<long, ITweet>();
        
        [HttpGet]
        public ActionResult Index(string info)
        {
            ViewBag.Info = info;
            
            if (info == null && MyCredentials.LastAuthenticatedCredentials == null)
            {
                ViewBag.Info = $"You need to authenticate first please visit : <a href=\"/Home/TwitterAuth\">http://{Request.Url?.Authority}/Home/TwitterAuth</a>";
            }
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Publish(string tweet, HttpPostedFileBase file)
        {
            var routeValueParameters = new RouteValueDictionary();
            var fileBytes = GetByteArrayFromFile(file);

            if (MyCredentials.LastAuthenticatedCredentials == null)
            {
                return RedirectToAction("Index", routeValueParameters);
            }
            
            if (tweet == null && fileBytes == null)
            {
                routeValueParameters.Add("info", "You must specify either ");
                return RedirectToAction("Index", routeValueParameters);
            }

            try
            {
                var client = new TwitterClient(MyCredentials.LastAuthenticatedCredentials);
                var publishTweetParameters = new PublishTweetParameters(tweet);
                if (fileBytes != null)
                {
                    publishTweetParameters.MediaBinaries.Add(fileBytes);
                }
                
                var publishedTweet = await client.Tweets.PublishTweet(publishTweetParameters);
                
                PostedTweets.Add(publishedTweet.Id, publishedTweet);
                routeValueParameters.Add("id", publishedTweet.Id);
                routeValueParameters.Add("actionPerformed", "Publish");
                routeValueParameters.Add("success", true);
            }
            catch (Exception)
            {
                routeValueParameters.Add("success", false);
            }

            return RedirectToAction("TweetPublished", routeValueParameters);
        }

        private byte[] GetByteArrayFromFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            file.InputStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public ActionResult TweetPublished(long id, string actionPerformed, bool success = true)
        {
            ViewBag.Tweet = PostedTweets[id];
            ViewBag.ActionType = actionPerformed;
            ViewBag.Success = success;
            return View();
        }

        public async Task<ActionResult> DeleteTweet(long id)
        {
            var routeValueParameters = new RouteValueDictionary();

            try
            {
                var client = new TwitterClient(MyCredentials.LastAuthenticatedCredentials);
                await client.Tweets.DestroyTweet(id);

                routeValueParameters.Add("id", id);
                routeValueParameters.Add("actionPerformed", "Delete");
                routeValueParameters.Add("success", true);
            }
            catch (Exception)
            {
                routeValueParameters.Add("success", false);
            }
          
            return RedirectToAction("TweetPublished", routeValueParameters);
        }
    }
}
