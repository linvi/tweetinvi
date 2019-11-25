using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.ASP.NET.Core.Controllers
{
    public class TweetController : Controller
    {
        private readonly ITwitterCredentials _credentials;

        public TweetController()
        {
            _credentials = MyCredentials.GenerateCredentials();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string tweet, IFormFile file)
        {
            var fileBytes = GetByteArrayFromFile(file);
            var client = new TwitterClient(_credentials);

            var media = await client.Upload.UploadBinary(fileBytes);
            var publishedTweet = await client.Tweets.PublishTweet(new PublishTweetParameters(tweet)
            {
                Medias = { media }
            });

            var routeValueParameters = new Dictionary<string, object>
            {
                { "id", publishedTweet?.Id },
                { "author", publishedTweet?.CreatedBy.ScreenName },
                { "actionPerformed", "Publish" },
                { "success", publishedTweet != null }
            };

            return RedirectToAction("TweetPublished", routeValueParameters);
        }

        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            file.OpenReadStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public ActionResult TweetPublished(long? id, string author, string actionPerformed, bool success = true)
        {
            ViewBag.TweetId = id;
            ViewBag.Author = author;
            ViewBag.ActionType = actionPerformed;
            ViewBag.Success = success;
            return View();
        }

        public ActionResult DeleteTweet(long id)
        {
            var client = new TwitterClient(_credentials);

            var success = Auth.ExecuteOperationWithCredentials(_credentials, () =>
            {
                var tweet = client.Tweets.GetTweet(id).Result;
                if (tweet != null)
                {
                    return tweet.Destroy().Result;
                }

                return false;
            });

            var routeValueParameters = new Dictionary<string, object>();
            routeValueParameters.Add("id", id);
            routeValueParameters.Add("actionPerformed", "Delete");
            routeValueParameters.Add("success", success);
            return RedirectToAction("TweetPublished", routeValueParameters);
        }
    }
}
