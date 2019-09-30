using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Exceptions;
using Tweetinvi.Json;
using Tweetinvi.Logic.Model;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
// REST API

// STREAM API
using Stream = Tweetinvi.Stream;

// Others
// Handle Exceptions
// Extension methods provided by Tweetinvi
// Data Transfer Objects for Serialization
using Geo = Tweetinvi.Geo;
using SavedSearch = Tweetinvi.SavedSearch;
// ReSharper disable StringLiteralTypo

// JSON static classes to get json from Twitter.

// ReSharper disable UnusedVariable
namespace Examplinvi.NETFramework
{
    // IMPORTANT 
    // This cheat sheet provide examples for all the features provided by Tweetinvi.

    // WINDOWS PHONE 8 developers
    // If you are a windows phone developer, please use the Async classes
    // User.GetAuthenticatedUser(); -> await UserAsync.GetAuthenticatedUser();

    class Program
    {
        public static ITwitterCredentials Credentials { get; set; }

        static void Main()
        {
            Credentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Auth.SetCredentials(Credentials);

            Examples.Client = new TwitterClient(Credentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine(args.Url);
            };

            var client = new TwitterClient(Credentials);
            var authenticatedUser = client.Account.GetAuthenticatedUser().Result;

            Console.WriteLine(authenticatedUser);

            // Un-comment to run the examples below
            // await Examples.ExecuteExamples = true;

            GenerateCredentialExamples().Wait();
            TweetExamples().Wait();
            UserExamples().Wait();
            AuthenticatedUserExamples().Wait();
            TimelineExamples().Wait();
            MessageExamples().Wait();
            TwitterListExamples().Wait();
            GeoExamples().Wait();
            SearchExamples().Wait();
            SavedSearchesExamples().Wait();
            RateLimitExamples().Wait();
            HelpExamples().Wait();
            JsonExamples().Wait();
            StreamExamples().Wait();
            AdditionalFeaturesExamples().Wait();
            Examples.ConfigureTweetinvi();
            Examples.GlobalEvents();
            UploadExamples().Wait();
            DownloadExamples().Wait();

            Console.WriteLine(@"END");
            Console.ReadLine();
        }

        #region Examples Store

        // ReSharper disable LocalizableElement
        // ReSharper disable UnusedMember.Local

        private static async Task GenerateCredentialExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            // With captcha
            await Examples.AuthFlow_WithCaptcha_StepByStep("consumer_key", "consumer_secret");

            // With callback URL
            await Examples.AuthFlow_CreateFromRedirectedCallbackURL_StepByStep("consumer_key", "consumer_secret");

            await Examples.AuthFlow_CreateFromRedirectedVerifierCode_StepByStep("consumer_key", "consumer_secret");
        }

        private static async Task TweetExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Tweet_GetExistingTweet(210462857140252672);

            await Examples.Tweet_PublishTweet($"I love tweetinvi! ({Guid.NewGuid()})");
            await Examples.Tweet_PublishTweetWithImage("superb file", "YOUR_FILE_PATH.png");

            await Examples.Tweet_PublishTweetInReplyToAnotherTweet($"I love tweetinvi! ({Guid.NewGuid()})", 392711547081854976);
            await Examples.Tweet_PublishTweetWithGeo($"I love tweetinvi! ({Guid.NewGuid()})");

            await Examples.Tweet_Destroy();

            await Examples.Tweet_GetRetweets(210462857140252672);
            await Examples.Tweet_PublishRetweet(210462857140252672);
            await Examples.Tweet_DestroyRetweet(210462857140252672);

            await Examples.Tweet_GenerateOEmbedTweet();
            await Examples.Tweet_SetTweetAsFavorite(392711547081854976);
        }

        private static async Task UserExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.User_GetCurrentUser();

            await Examples.User_GetUserFromId(1478171);
            await Examples.User_GetUserFromName(Examples.USER_SCREEN_NAME_TO_TEST);

            await Examples.User_GetFavorites(Examples.USER_SCREEN_NAME_TO_TEST);

            await Examples.User_GetFriends(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.User_GetFriendIds(Examples.USER_SCREEN_NAME_TO_TEST);

            await Examples.User_GetFollowers(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.User_GetFollowerIds(Examples.USER_SCREEN_NAME_TO_TEST);

            await Examples.User_GetRelationshipBetween("tweetinvitest", Examples.USER_SCREEN_NAME_TO_TEST);

            await Examples.User_BlockUser(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.User_UnBlockUser(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.User_GetBlockedUsers();

            await Examples.User_DownloadProfileImage(Examples.USER_SCREEN_NAME_TO_TEST);
        }

        private static async Task AuthenticatedUserExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Friendship_GetMultipleRelationships();
            await Examples.AuthenticatedUser_GetIncomingRequests();
            await Examples.AuthenticatedUser_GetOutgoingRequests();
            await Examples.AuthenticatedUser_FollowUser(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.AuthenticatedUser_UnFollowUser(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.AuthenticatedUser_UpdateFollowAuthorizationsForUser(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.AuthenticatedUser_GetLatestMessages();
            await Examples.AuthenticatedUser_GetAccountSettings();
        }

        private static async Task TimelineExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Timeline_GetHomeTimeline();
            await Examples.Timeline_GetUserTimeline(Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.Timeline_GetMentionsTimeline();
        }

        private static async Task MessageExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.AuthenticatedUser_GetLatestMessages();

            await Examples.Message_GetLatests();
            await Examples.Message_GetMessageFromId(381069551028293633);
            await Examples.Message_DestroyMessageFromId(42);
            await Examples.Message_PublishMessage("I love tweetinvi", Examples.USER_SCREEN_NAME_TO_TEST);
            await Examples.Message_PublishMessageWithImage("I love attachments", Examples.USER_SCREEN_NAME_TO_TEST,
                "./path_to_image_file");
            await Examples.Message_PublishMessageWithQuickReplyOptions();
        }

        private static async Task StreamExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Stream_SampleStreamExample();
            await Examples.Stream_FilteredStreamExample();
            Examples.SimpleStream_Events();
        }

        private static async Task TwitterListExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.TwitterList_GetUserOwnedLists();
            await Examples.TwitterList_GetUserSubscribedLists();

            await Examples.TwitterList_CreateList();
            await Examples.TwitterList_GetExistingListById(105601767);
            await Examples.TwitterList_UpdateList(105601767);
            await Examples.TwitterList_DestroyList(105601767);
            await Examples.TwitterList_GetTweetsFromList(105601767);
            await Examples.TwitterList_GetMembersOfList(105601767);
        }

        private static async Task GeoExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Geo_GetPlaceFromId("df51dec6f4ee2b2c");
            await Examples.Trends_GetTrendsFromWoeId(1);
        }

        private static async Task SearchExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Search_SimpleTweetSearch();
            await Examples.Search_SearchTweet();
            await Examples.Search_SearchWithMetadata();
            await Examples.Search_FilteredSearch();
            await Examples.Search_SearchAndGetMoreThan100Results();
            await Examples.Search_SearchUsers();
        }

        private static async Task SavedSearchesExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.SavedSearch_CreateSavedSearch("tweetinvi");
            await Examples.SavedSearch_GetSavedSearches();
            await Examples.SavedSearch_GetSavedSearch(307102135);
            await Examples.SavedSearch_DestroySavedSearch(307102135);
        }

        private static async Task RateLimitExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.RateLimits_Track_Examples();
            Examples.RateLimits_ManualAwait();

            await Examples.GetCredentialsRateLimits();
            await Examples.GetCurrentCredentialsRateLimits();
        }

        private static async Task HelpExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.GetTwitterPrivacyPolicy();
        }

        private static async Task JsonExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Json_GetJsonForMessageRequestExample();
            await Examples.Json_GetJsonForGeoRequestExample();
            await Examples.Json_GetJsonForHelpRequestExample();
            await Examples.Json_GetJsonForSavedSearchRequestExample();
            await Examples.Json_GetJsonForTimelineRequestExample();
            await Examples.Json_GetJsonForTrendsRequestExample();
            await Examples.Json_GetJsonForTweetRequestExample();
        }

        private static async Task AdditionalFeaturesExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.Exceptions_GetExceptionsInfo();
            await Examples.ManualQuery_Example();
        }

        private static async Task UploadExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            await Examples.UploadImage("./path_to_image_file");
            await Examples.UploadImage("./path_to_gif_file");
            await Examples.UploadVideo("./path_to_video_file");
            await Examples.Tweet_PublishTweetWithImage("publish with img", "filePath");
        }

        private static async Task DownloadExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            var binary = await Examples.DownloadBinaryFromTwitter("https://ton.twitter.com/1.1/ton/data/dm/764104492082233347/764104492107370496/X0v8XTZ4.jpg");
        }

        #endregion
    }

    static class Examples
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static bool ExecuteExamples { get; set; }
        public const string USER_SCREEN_NAME_TO_TEST = "ladygaga";
        public static TwitterClient Client { get; set; }

        #region Credentials and Login

        // Get credentials with captcha system
        // ReSharper disable UnusedMethodReturnValue.Local
        public static async Task<ITwitterCredentials> AuthFlow_WithCaptcha_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = new ConsumerCredentials(consumerKey, consumerSecret);
            var authenticationContext = await AuthFlow.InitAuthentication(applicationCredentials);
            Console.WriteLine("Go on : {0}", authenticationContext.AuthorizationURL);
            Console.WriteLine("Enter the captcha : ");
            var captcha = Console.ReadLine();

            try
            {
                var newCredentials = await AuthFlow.CreateCredentialsFromVerifierCode(captcha, authenticationContext);
                Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
                Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

                return newCredentials;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Get credentials with callbackURL system
        public static async Task<ITwitterCredentials> AuthFlow_CreateFromRedirectedCallbackURL_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = new ConsumerCredentials("YHGdHYh7J464jl6Uk38jLRCvq", "lqKIkby71YV7L7IItQpIOVuyLU9HVIgTinz4f6c0a0yUeT6Pj0");
            var authenticationContext = await AuthFlow.InitAuthentication(applicationCredentials, "http://www.linvi.net");
            Console.WriteLine($"Redirecting to {authenticationContext.AuthorizationURL}");
            Console.WriteLine("Go on : {0}", authenticationContext);
            Console.WriteLine("When redirected to your website copy and paste the URL: ");

            // Enter a value like: http://www.linvi.net?oauth_token={tokenValue}&oauth_verifier={verifierValue}

            var callbackURL = Console.ReadLine();

            // Here we provide the entire URL where the user has been redirected
            var newCredentials = await AuthFlow.CreateCredentialsFromCallbackURL(callbackURL, authenticationContext);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        public static async Task<ITwitterCredentials> AuthFlow_CreateFromRedirectedVerifierCode_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = new ConsumerCredentials(consumerKey, consumerSecret);
            var authenticationContext = await AuthFlow.InitAuthentication(applicationCredentials, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Go on : {0}", authenticationContext);
            Console.WriteLine("When redirected to your website copy and paste the value of the oauth_verifier : ");

            // For the following redirection https://tweetinvi.codeplex.com?oauth_token=UR3eTEwDXFNhkMnjqz0oFbRauvAm4YhnF67KE6hO8Q&oauth_verifier=woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A
            // Enter the value : woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A

            var verifierCode = Console.ReadLine();

            // Here we only provide the verifier code
            var newCredentials = await AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authenticationContext);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        // ReSharper restore UnusedMethodReturnValue.Local

        #endregion

        #region Tweet

        public static async Task Tweet_GetExistingTweet(long tweetId)
        {
            var tweet = await Client.Tweets.GetTweet(tweetId);
            Console.WriteLine(tweet.Text);
        }

        public static async Task Tweet_PublishTweet(string text)
        {
            var tweet = await Client.Tweets.PublishTweet(text);
            Console.WriteLine(tweet.IsTweetPublished);
        }

        public static async Task<ITweet> Tweet_PublishTweetWithImage(string text, string filePath)
        {
            var media = await UploadImage(filePath);

            return await Client.Tweets.PublishTweet(new PublishTweetParameters(text)
            {
                Medias = new List<IMedia>() { media }
            });
        }

        public static async Task Tweet_PublishTweetInReplyToAnotherTweet(string text, long tweetIdToReplyTo)
        {
            // With the new version of Twitter you no longer have to specify the mentions. Twitter can do that for you automatically.
            var reply = await Client.Tweets.PublishTweet(new PublishTweetParameters(text)
            {
                InReplyToTweetId = tweetIdToReplyTo,
                AutoPopulateReplyMetadata = true // Auto populate the @mentions
            });

            var tweetToReplyTo = await Client.Tweets.GetTweet(tweetIdToReplyTo);

            // We must add @screenName of the author of the tweet we want to reply to
            var textToPublish = $"@{tweetToReplyTo.CreatedBy.ScreenName} {text}";
            var tweet = await Client.Tweets.PublishTweet(new PublishTweetParameters
            {
                Text = textToPublish,
                InReplyToTweetId = tweetIdToReplyTo
            });

            Console.WriteLine($"Publish success? {tweet != null}");
        }

        public static async Task Tweet_PublishTweetWithGeo(string text)
        {
            const double latitude = 37.7821120598956;
            const double longitude = -122.400612831116;

            var publishParameters = new PublishTweetParameters(text)
            {
                Coordinates = new Coordinates(latitude, longitude)
            };

            var tweet = await Client.Tweets.PublishTweet(publishParameters);

            Console.WriteLine(tweet.IsTweetPublished);
        }

        public static async Task Tweet_PublishRetweet(long tweetId)
        {
            var tweet = await Client.Tweets.GetTweet(tweetId);
            var retweet = await tweet.PublishRetweet();

            Console.WriteLine("You retweeted : '{0}'", retweet.Text);
        }

        public static async Task Tweet_DestroyRetweet(long tweetId)
        {
            var tweet = await Client.Tweets.GetTweet(tweetId);
            var retweet = await tweet.PublishRetweet();

            await retweet.Destroy();
        }

        public static async Task Tweet_GetRetweets(long tweetId)
        {
            var tweet = await Client.Tweets.GetTweet(tweetId);
            IEnumerable<ITweet> retweets = await tweet.GetRetweets();

            var firstRetweeter = retweets.ElementAt(0).CreatedBy;
            var originalTweet = retweets.ElementAt(0).RetweetedTweet;
            Console.WriteLine($"{firstRetweeter.Name} retweeted : '{originalTweet.Text}'");
        }

        public static async Task Tweet_GenerateOEmbedTweet()
        {
            var newTweet = await Client.Tweets.PublishTweet("to be oembed");
            var oembedTweet = await newTweet.GenerateOEmbedTweet();

            Console.WriteLine("Oembed tweet url : {0}", oembedTweet.URL);

            if (newTweet.IsTweetPublished)
            {
                await newTweet.Destroy();
            }
        }

        public static async Task Tweet_Destroy()
        {
            var newTweet = await Client.Tweets.PublishTweet("to be destroyed!");
            bool isTweetPublished = newTweet.IsTweetPublished;

            if (isTweetPublished)
            {
                await newTweet.Destroy();
            }

            bool tweetDestroyed = newTweet.IsTweetDestroyed;
            Console.WriteLine("Has the tweet destroyed? {0}", tweetDestroyed);
        }

        public static async Task Tweet_SetTweetAsFavorite(long tweetId)
        {
            var tweet = await Client.Tweets.GetTweet(tweetId);
            await tweet.Favorite();

            Console.WriteLine("Is tweet now favourite? -> {0}", tweet.Favorited);
        }

        #endregion

        #region User

        public static async Task User_GetCurrentUser()
        {
            var client = new TwitterClient(Program.Credentials);
            var user = await client.Account.GetAuthenticatedUser();
            Console.WriteLine(user.ScreenName);
        }

        public static async Task User_GetUserFromId(long userId)
        {
            var user = await Client.Users.GetUser(userId);
            Console.WriteLine(user.ScreenName);
        }

        public static async Task User_GetUserFromName(string username)
        {
            var user = await Client.Users.GetUser(username);
            Console.WriteLine(user.Id);
        }

        public static async Task User_GetFriendIds(string username)
        {
            var friendIdsIterator = Client.Users.GetFriendIds(new GetFriendIdsParameters(username)
            {
                PageSize = 50
            });

            var allFriendIds = new List<long>();

            while (allFriendIds.Count < 200 && !friendIdsIterator.Completed)
            {
                var pageOfFriendIds = await friendIdsIterator.MoveToNextPage();
                allFriendIds.AddRange(pageOfFriendIds);
            }

            Console.WriteLine($"{username} has friends, here are some of them :");

            foreach (var friendId in allFriendIds)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        public static async Task User_GetFriends(string username)
        {
            var user = await Client.Users.GetUser(username);

            var friendsIterator = user.GetFriends();
            var allFriends = new List<IUser>();

            while (allFriends.Count < 200 && !friendsIterator.Completed)
            {
                var pageOfFriends = await friendsIterator.MoveToNextPage();
                allFriends.AddRange(pageOfFriends);
            }

            Console.WriteLine($"{username} has friends, here are some of them :");

            foreach (var friendId in allFriends)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        public static async Task User_GetFollowerIds(string username)
        {
            var followersIterator = Client.Users.GetFollowerIds(username);
            var allFollowerIds = new List<long>();

            while (allFollowerIds.Count < 200 && !followersIterator.Completed)
            {
                var pageOfFollowerIds = await followersIterator.MoveToNextPage();
                allFollowerIds.AddRange(pageOfFollowerIds);
            }

            Console.WriteLine($"{username} has some followers, here are some of them :");

            foreach (var followerId in allFollowerIds)
            {
                Console.WriteLine("- {0}", followerId);
            }
        }

        public static async Task User_GetFollowers(string username)
        {
            var user = await Client.Users.GetUser(username);
            var followersIterator = user.GetFollowers();
            var allFollowers = new List<IUser>();

            while (allFollowers.Count < 200 && !followersIterator.Completed)
            {
                var pageOfFollowers = await followersIterator.MoveToNextPage();
                allFollowers.AddRange(pageOfFollowers);
            }

            Console.WriteLine($"{username} has some followers, here are some of them :");

            foreach (var follower in allFollowers)
            {
                Console.WriteLine("- {0}", follower);
            }
        }

        public static async Task User_GetRelationshipBetween(string sourceusername, string targetusername)
        {
            var sourceUser = await Client.Users.GetUser(sourceusername);
            var targetUser = await Client.Users.GetUser(targetusername);

            var relationship = await sourceUser.GetRelationshipWith(targetUser);
            Console.WriteLine("You are{0} following {1}", relationship.Following ? "" : " not", targetusername);
            Console.WriteLine("You are{0} being followed by {1}", relationship.FollowedBy ? "" : " not", targetusername);
        }

        public static async Task User_GetFavorites(string username)
        {
            var user = await Client.Users.GetUser(username);
            var favoritesIterator = user.GetFavoriteTweets();
            var favoriteTweets = new List<ITweet>();

            while (!favoritesIterator.Completed)
            {
                var page = await favoritesIterator.MoveToNextPage();
                favoriteTweets.AddRange(page);
            }

            foreach (var favoriteTweet in favoriteTweets)
            {
                Console.WriteLine("- {0}", favoriteTweet.Text);
            }
        }

        public static async Task User_BlockUser(string username)
        {
            var user = await Client.Users.GetUser(username);

            if (await user.BlockUser())
            {
                Console.WriteLine("{0} has been blocked.", username);
            }
            else
            {
                Console.WriteLine("{0} has not been blocked.", username);
            }
        }

        public static async Task User_UnBlockUser(string username)
        {
            var user = await Client.Users.GetUser(username);
            await user.UnBlockUser();
        }

        public static async Task User_GetBlockedUsers()
        {
            var client = new TwitterClient(Program.Credentials);
            var authenticatedUser = await client.Account.GetAuthenticatedUser();

            var blockedUserIterator = authenticatedUser.GetBlockedUsers();
            var someBlockedUsers = await blockedUserIterator.MoveToNextPage();

            Console.WriteLine(string.Concat(someBlockedUsers.Select(x => x.ToString())));

            var blockedUserIdsIterator = authenticatedUser.GetBlockedUserIds();
            var someBlockedUserIds = await blockedUserIdsIterator.MoveToNextPage();

            Console.WriteLine(string.Concat(someBlockedUserIds.Select(x => x.ToString())));
        }

        public static async Task User_DownloadProfileImage(string username)
        {
            var user = await Client.Users.GetUser(username);
            var stream = await user.GetProfileImageStream(ImageSize.bigger);

            using (var fileStream = new FileStream($"{user.Id}.jpg", FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

#if NET_CORE
            string assemblyPath = Path.GetDirectoryName(typeof(User).GetTypeInfo().Assembly.CodeBase);
#else
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
#endif

            if (assemblyPath != null)
            {
                Process.Start(assemblyPath);
            }
        }

        public static async Task User_GetMutedUsers()
        {
            var client = new TwitterClient(Program.Credentials);
            var authenticatedUser = await client.Account.GetAuthenticatedUser();
            var mutedUsed = await authenticatedUser.GetMutedUserIds();

            Console.WriteLine($"Muted user ids : {string.Concat(mutedUsed.Select(x => x.ToString() + " ; "))}");
        }

        #endregion

        #region AuthenticatedUser

        public static async Task AuthenticatedUser_GetIncomingRequests()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var iterator = authenticatedUser.GetUserIdsRequestingFriendship();
            var ids = new List<long>();

            while (!iterator.Completed)
            {
                ids.AddRange(await iterator.MoveToNextPage());
            }

            foreach (var userIds in ids)
            {
                Console.WriteLine("{0} wants to follow you!", userIds);
            }
        }

        public static async Task AuthenticatedUser_GetOutgoingRequests()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var usersRequestingFriendshipIterator = authenticatedUser.GetUsersYouRequestedToFollow();

            var usersRequestingFriendship = new List<IUser>();

            while (!usersRequestingFriendshipIterator.Completed)
            {
                var page = await usersRequestingFriendshipIterator.MoveToNextPage();
                usersRequestingFriendship.AddRange(page);
            }

            foreach (var user in usersRequestingFriendship)
            {
                Console.WriteLine("You sent a request to follow {0}!", user.Name);
            }
        }

        public static async Task AuthenticatedUser_FollowUser(string username)
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var userToFollow = await Client.Users.GetUser(username);

            if (await authenticatedUser.FollowUser(userToFollow))
            {
                Console.WriteLine("You have successfully sent a request to follow {0}", userToFollow.Name);
            }
        }

        public static async Task AuthenticatedUser_UnFollowUser(string username)
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var userToFollow = await Client.Users.GetUser(username);

            if (await authenticatedUser.UnFollowUser(userToFollow))
            {
                Console.WriteLine("You are not following {0} anymore", userToFollow.Name);
            }
        }

        public static async Task AuthenticatedUser_UpdateFollowAuthorizationsForUser(string username)
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            await authenticatedUser.UpdateRelationship(new UpdateRelationshipParameters(username)
            {
                EnableRetweets = false,
                EnableDeviceNotifications = true
            });
        }

        public static async Task AuthenticatedUser_GetLatestMessages()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var messages = await authenticatedUser.GetLatestMessages(20);

            Console.WriteLine("Messages : ");
            foreach (var message in messages)
            {
                Console.WriteLine("- '{0}'", message.Text);
            }
        }

        public static async Task AuthenticatedUser_GetAccountSettings()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var settings = await authenticatedUser.GetAccountSettings();

            Console.WriteLine("{0} uses lang : {1}", settings.ScreenName, settings.Language);
        }

        #endregion

        #region Timeline

        public static async Task Timeline_GetUserTimeline(string username)
        {
            var user = await Client.Users.GetUser(username);

            var timelineTweets = await user.GetUserTimeline();
            foreach (var tweet in timelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        public static async Task Timeline_GetHomeTimeline()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();

            var homeTimelineTweets = await authenticatedUser.GetHomeTimeline();
            foreach (var tweet in homeTimelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        public static async Task Timeline_GetMentionsTimeline()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();

            var mentionsTimelineTweets = await authenticatedUser.GetMentionsTimeline();
            foreach (var mention in mentionsTimelineTweets)
            {
                Console.WriteLine(mention.Text);
            }
        }

        #endregion

        #region Stream

        public static void SimpleStream_Events()
        {
            var stream = Stream.CreateTweetStream();
            stream.TweetReceived += (sender, args) => { };
            stream.TweetDeleted += (sender, args) => { };
            stream.TweetLocationInfoRemoved += (sender, args) => { };

            stream.TweetWitheld += (sender, args) => { };
            stream.UserWitheld += (sender, args) => { };

            stream.LimitReached += (sender, args) => { };
            stream.DisconnectMessageReceived += (sender, args) => { };
            stream.WarningFallingBehindDetected += (sender, args) => { };

            stream.UnmanagedEventReceived += (sender, args) => { };
        }

        public static async Task Stream_SampleStreamExample()
        {
            var stream = Stream.CreateSampleStream();

            stream.TweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet.Text);
            };

            stream.AddTweetLanguageFilter(LanguageFilter.English);
            stream.AddTweetLanguageFilter(LanguageFilter.French);

            await stream.StartStream();
        }

        public static async Task Stream_FilteredStreamExample()
        {
            var stream = Stream.CreateFilteredStream();
            var location = new Location(36.8, -124.75, 32.75, -126.89);

            stream.AddLocation(location);
            stream.AddTrack("tweetinvi");
            stream.AddTrack("linvi");

            stream.MatchingTweetReceived += (sender, args) =>
            {
                var tweet = args.Tweet;
                Console.WriteLine("{0} was detected between the following tracked locations:", tweet.Id);

                IEnumerable<ILocation> matchingLocations = args.MatchingLocations;
                foreach (var matchingLocation in matchingLocations)
                {
                    Console.Write("({0}, {1}) ;", matchingLocation.Coordinate1.Latitude, matchingLocation.Coordinate1.Longitude);
                    Console.WriteLine("({0}, {1})", matchingLocation.Coordinate2.Latitude, matchingLocation.Coordinate2.Longitude);
                }
            };

            await stream.StartStreamMatchingAllConditions();
        }

        #endregion

        #region Search

        public static async Task Search_SimpleTweetSearch()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!
            var tweets = await Search.SearchTweets("#obama");

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}", tweet.Text);
            }
        }

        public static async Task Search_SearchTweet()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!

            var searchParameter = Search.CreateTweetSearchParameter("obama");

            searchParameter.SetGeoCode(new Coordinates(37.781157, -122.398720), 1, DistanceMeasure.Miles);
            searchParameter.Lang = LanguageFilter.English;
            searchParameter.SearchType = SearchResultType.Popular;
            searchParameter.MaximumNumberOfResults = 100;
            searchParameter.Since = new DateTime(2013, 12, 1);
            searchParameter.Until = new DateTime(2013, 12, 11);
            searchParameter.SinceId = 399616835892781056;
            searchParameter.MaxId = 405001488843284480;
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
            searchParameter.Filters = TweetSearchFilters.Videos;

            var tweets = await Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static async Task Search_SearchWithMetadata()
        {
            await Search.SearchTweetsWithMetadata("hello");
        }

        public static async Task Search_FilteredSearch()
        {
            var searchParameter = Search.CreateTweetSearchParameter("#tweetinvi");
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;

            var tweets = await Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static async Task Search_SearchAndGetMoreThan100Results()
        {
            var searchParameter = Search.CreateTweetSearchParameter("us");
            searchParameter.MaximumNumberOfResults = 200;

            var tweets = await Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static async Task Search_SearchUsers()
        {
            var users = await Search.SearchUsers("linvi", 100);
            users.ForEach(Console.WriteLine);
        }

        #endregion

        #region Saved Searches

        public static async Task SavedSearch_GetSavedSearches()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var savedSearches = await authenticatedUser.GetSavedSearches();

            Console.WriteLine("Saved Searches");
            foreach (var savedSearch in savedSearches)
            {
                Console.WriteLine("- {0} => {1}", savedSearch.Name, savedSearch.Query);
            }
        }

        public static async Task SavedSearch_CreateSavedSearch(string query)
        {
            var savedSearch = await SavedSearch.CreateSavedSearch(query);
            Console.WriteLine("Saved Search created as : {0}", savedSearch.Name);
        }

        public static async Task SavedSearch_GetSavedSearch(long searchId)
        {
            var savedSearch = await SavedSearch.GetSavedSearch(searchId);
            Console.WriteLine("Saved searched query is : '{0}'", savedSearch.Query);
        }

        public static async Task SavedSearch_DestroySavedSearch(long searchId)
        {
            var savedSearch = await SavedSearch.GetSavedSearch(searchId);

            if (await SavedSearch.DestroySavedSearch(savedSearch))
            {
                Console.WriteLine("You destroyed the search successfully!");
            }
        }

        #endregion

        #region Message

        public static async Task Message_GetLatests()
        {
            // Messages Sent or received
            var asyncCursorResult = await Message.GetLatestMessagesWithCursor();
            var cursor = asyncCursorResult.Cursor;

            // Check for a cursor having been returned, if not, there's no more results
            if (cursor == null)
            {
                return;
            }

            // Fetch more results using the cursor (note: Tweetinvi will automatically do this internally if you just specify
            //  a count larger than TweetinviConsts.MESSAGE_GET_COUNT in your initial request, but you may want to do it
            //  manually for large requests to work within rate limits)
            var latestMessagesParameters = new GetMessagesParameters()
            {
                Count = 20,
                Cursor = cursor
            };

            var latestMessagesFromParameters = await Message.GetLatestMessages(latestMessagesParameters);
        }

        public static async Task Message_GetMessageFromId(long messageId)
        {
            var message = await Message.GetExistingMessage(messageId);
            Console.WriteLine("Message from {0} to {1} : {2}", message.SenderId, message.RecipientId, message.Text);
        }

        public static async Task Message_DestroyMessageFromId(long messageId)
        {
            var message = await Message.GetExistingMessage(messageId);
            if (await message.Destroy())
            {
                Console.WriteLine("Message successfully destroyed!");
            }
        }

        public static async Task Message_PublishMessage(string text, string username)
        {
            var recipient = await Client.Users.GetUser(username);
            var message = await Message.PublishMessage(text, recipient.Id);

            if (message != null)
            {
                Console.WriteLine("Message published with ID {0}", message.Id);
            }
        }

        public static async Task Message_PublishMessageWithImage(string text, string username, string imgPath)
        {
            // Get the user to DM
            var recipient = Client.Users.GetUser(username);

            // Get the image to attach from the local filesystem
            var imageBinary = File.ReadAllBytes(imgPath);

            // Upload the image to Twitter
            var uploadMediaParams = new UploadParameters(imageBinary)
            {
                // Note that the media category must be set to the Dm prefixed variant of whatever
                //  category of media you are uploading
                MediaCategory = Tweetinvi.Core.Public.Models.Enum.MediaCategory.DmImage
            };

            var media = await Client.Upload.UploadBinary(uploadMediaParams);

            // Publish the DM
            var publishMsgParams = new PublishMessageParameters(text, recipient.Id)
            {
                AttachmentMediaId = media.Id
            };
            var message = Message.PublishMessage(publishMsgParams);

            if (message != null)
            {
                Console.WriteLine("Message published with ID {0}", message.Id);
            }
        }

        public static async Task Message_PublishMessageWithQuickReplyOptions()
        {
            // Get the user to DM
            var recipient = await Client.Users.GetUser(USER_SCREEN_NAME_TO_TEST);

            // Publish the DM
            var publishMsgParams = new PublishMessageParameters("Do you like cheese?", recipient.Id)
            {
                QuickReplyOptions = new IQuickReplyOption[]
                {
                    new QuickReplyOption()
                    {
                        Label = "Yes",
                        Description = "Yes, I love cheese!",
                        Metadata = "1"
                    },
                    new QuickReplyOption()
                    {
                        Label = "No",
                        Description = "No, I do not love cheese...",
                        Metadata = "0"
                    }
                }
            };
            var message = await Message.PublishMessage(publishMsgParams);

            if (message != null)
            {
                Console.WriteLine("Message published with ID {0}", message.Id);
            }
        }

        #endregion

        #region Lists

        public static async Task TwitterList_GetUserOwnedLists()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var ownedLists = await TwitterList.GetUserOwnedLists(authenticatedUser);

            ownedLists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        public static async Task TwitterList_GetUserSubscribedLists()
        {
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var lists = await TwitterList.GetUserSubscribedLists(authenticatedUser);

            lists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        public static async Task TwitterList_GetExistingListById(long listId)
        {
            var list = await TwitterList.GetExistingList(listId);
            Console.WriteLine("You have retrieved the list {0}", list.Name);
        }

        public static async Task TwitterList_CreateList()
        {
            var list = await TwitterList.CreateList("plop", PrivacyMode.Public, "description");
            Console.WriteLine("List '{0}' has been created!", list.FullName);
        }

        public static async Task TwitterList_UpdateList(long listId)
        {
            var list = await TwitterList.GetExistingList(listId);
            var updateParameters = new TwitterListUpdateParameters
            {
                Name = "piloupe",
                Description = "pilouping description",
                PrivacyMode = PrivacyMode.Private
            };

            await list.Update(updateParameters);

            Console.WriteLine("List new name is : {0}", list.Name);
        }

        public static async Task TwitterList_DestroyList(long listId)
        {
            var list = await TwitterList.GetExistingList(listId);
            var hasBeenDestroyed = await list.Destroy();
            Console.WriteLine("Tweet {0} been destroyed.", hasBeenDestroyed ? "has" : "has not");
        }

        public static async Task TwitterList_GetTweetsFromList(long listId)
        {
            var list = await TwitterList.GetExistingList(listId);
            var tweets = await list.GetTweets();

            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static async Task TwitterList_GetMembersOfList(long listId)
        {
            var list = await TwitterList.GetExistingList(listId);
            var members = await list.GetMembers();

            members.ForEach(x => Console.WriteLine(x.Name));
        }

        public static async Task TwitterList_CheckUserMembership(long userId, long listId)
        {
            var isUserAMember = await TwitterList.CheckIfUserIsAListMember(userId, listId);
            Console.WriteLine("{0} is{1}a member of list {2}", userId, isUserAMember ? " " : " NOT ", listId);
        }

        public static async Task TwitterList_GetSubscribers(long listId)
        {
            var subscribers = await TwitterList.GetListSubscribers(listId);

            subscribers.ForEach(user => Console.WriteLine(user));
        }

        public static void TwitterList_SubscribeOrUnsubscribeToList(long listId)
        {
            var hasSuccessfullySubscribed = TwitterList.SubscribeAuthenticatedUserToList(listId);
            var hasUnsubscribed = TwitterList.UnSubscribeAuthenticatedUserToList(listId);
        }

        public static async Task TwitterList_CheckUserSubscription(long userId, long listId)
        {
            var isUserASubscriber = await TwitterList.CheckIfUserIsAListSubscriber(userId, listId);
            Console.WriteLine("{0} is{1}subscribed to the list {2}", userId, isUserASubscriber ? " " : " NOT ", listId);
        }

        #endregion

        #region Geo/Trends

        public static async Task Geo_GetPlaceFromId(string placeId)
        {
            var place = await Geo.GetPlaceFromId(placeId);
            Console.WriteLine(place.Name);
        }

        public static async Task Trends_GetTrendsFromWoeId(long woeid)
        {
            var placeTrends = await Trends.GetTrendsAt(woeid);
            Console.WriteLine(placeTrends.woeIdLocations.First().Name);
        }

        #endregion

        #region Rate Limits

        public static async Task RateLimits_Track_Examples()
        {
            // Enable Tweetinvi RateLimit Handler
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            // Get notified when your application is being stopped to wait for RateLimits to be available
            RateLimit.QueryAwaitingForRateLimit += (sender, args) =>
            {
                Console.WriteLine("{0} is awaiting {1}ms for RateLimit to be available", args.Query, args.ResetInMilliseconds);
            };

            // Get the RateLimit associated with a query, this can return null
            var queryRateLimit = await RateLimit.GetQueryRateLimit("https://api.twitter.com/1.1/application/rate_limit_status.json");

            // Pause the current thread until the specific RateLimit can be used
            RateLimit.AwaitForQueryRateLimit(queryRateLimit);

            // This stop the execution of the current thread until the RateLimits are available 
            RateLimit.AwaitForQueryRateLimit("https://api.twitter.com/1.1/application/rate_limit_status.json");

            // Tweetinvi uses a cache mechanism to store credentials RateLimits, this operation allows you to clear it.
            // If the UseRateLimitAwaiter option is enabled this will result in the RateLimits to be retrieved during the next query.
            RateLimit.ClearRateLimitCache();
        }

        public static void RateLimits_ManualAwait()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                var queryRateLimit = RateLimit.GetQueryRateLimit(args.Url).Result;
                RateLimit.AwaitForQueryRateLimit(queryRateLimit);
            };
        }

        public static async Task GetCurrentCredentialsRateLimits()
        {
            var tokenRateLimits = await RateLimit.GetCurrentCredentialsRateLimits();

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToString("T"));
        }

        public static async Task GetCredentialsRateLimits()
        {
            var credentials = Auth.CreateCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var tokenRateLimits = await RateLimit.GetCredentialsRateLimits(credentials);

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToString("T"));
        }

        #endregion

        #region Account & Relationships

        public static async Task Account_GetAndSetAccountSettings()
        {
            var settings = await Account.GetCurrentAccountSettings();

            var updatedSettingsRequestParameter = Account.CreateUpdateAccountSettingsRequestParameters(settings);
            updatedSettingsRequestParameter.SleepTimeEnabled = false;

            var updatedSettings = await Account.UpdateAccountSettings(updatedSettingsRequestParameter);
            Console.WriteLine(updatedSettings.SleepTimeEnabled);
        }

        public static void Account_Mute()
        {
            // Get
            Account.GetMutedUserIds();
            Account.GetMutedUsers();

            // Create
            Account.MuteUser("username");

            // Delete
            Account.UnMuteUser("username");
        }

        public static async Task Friendship_GetMultipleRelationships()
        {
            var user1 = await Client.Users.GetUser("tweetinviapi");
            var user2 = await Client.Users.GetUser(USER_SCREEN_NAME_TO_TEST);

            var users = new List<IUser>
            {
                user1,
                user2
            };

            var relationships = await Client.Account.GetRelationshipsWith(users.ToArray());

            foreach (var user in users)
            {
                var relationship = relationships[user];
                Console.WriteLine("You are{0} following {1}", relationship.Following ? "" : " not", relationship.TargetScreenName);
                Console.WriteLine("You are{0} being followed by {1}", relationship.FollowedBy ? "" : " not", relationship.TargetScreenName);
                Console.WriteLine();
            }
        }

        public static async Task Friendship_GetUsersYouRequestedToFollow()
        {
            var usersYouWantToFollowIterator = Client.Account.GetUsersYouRequestedToFollow();
            var usersYouWantToFollow = new List<IUser>();

            while (!usersYouWantToFollowIterator.Completed)
            {
                var page = await usersYouWantToFollowIterator.MoveToNextPage();
                usersYouWantToFollow.AddRange(usersYouWantToFollow);
            }

            foreach (var user in usersYouWantToFollow)
            {
                Console.WriteLine("You want to be friend with {0}.", user.Name);
            }
        }

        public static async Task Friendship_GetUsersWhoseRetweetsAreMuted()
        {
            var userIds = await Client.Account.GetUserIdsWhoseRetweetsAreMuted();
            Console.WriteLine($"Muted users = {string.Join(", ", userIds)}");
        }

        public static async Task Friendship_UpdateRelationship()
        {
            var success = await Client.Account.UpdateRelationship(new UpdateRelationshipParameters("tweetinvitest")
            {
                EnableRetweets = false
            });

            Console.WriteLine($"Update of relationship success = ${success}");
        }

        public static async Task Friendship_GetRelationshipDetails()
        {
            var relationshipDetails = await Client.Users.GetRelationshipBetween("tweetinviapi", "twitterapi");
            Console.WriteLine(relationshipDetails.BlockedBy);
        }

        #endregion

        #region Upload

        public static async Task<IMedia> UploadImage(string filepath)
        {
            var imageBinary = File.ReadAllBytes(filepath);
            var media = await Client.Upload.UploadBinary(imageBinary);

            return media;
        }

        public static async Task<IMedia> UploadGif(string filepath)
        {
            var gifBinary = File.ReadAllBytes(filepath);
            var media = await Client.Upload.UploadBinary(gifBinary);

            return media;
        }

        public static async Task<IMedia> UploadVideo(string filepath)
        {
            var videoBinary = File.ReadAllBytes(filepath);

            var media = await Client.Upload.UploadVideo(new UploadVideoParameters(videoBinary)
            {
                UploadStateChanged = uploadChangeEventArgs =>
                {
                    Console.WriteLine(uploadChangeEventArgs.Percentage);
                }
            });

            return media;
        }

        #endregion

        #region Download

        public static Task<byte[]> DownloadBinaryFromTwitter(string twitterUrl)
        {
            return TwitterAccessor.DownloadBinary(twitterUrl);
        }

        #endregion

        #region Json
        public static async Task Json_GetJsonForMessageRequestExample()
        {
            IUser user = await Client.Users.GetUser("tweetinviapi");
            string jsonResponse = await MessageJson.PublishMessage("salut", user.Id);

            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForGeoRequestExample()
        {
            var jsonResponse = await GeoJson.GetPlaceFromId("df51dec6f4ee2b2c");
            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForHelpRequestExample()
        {
            var jsonResponse = await HelpJson.GetCredentialsRateLimits();
            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForSavedSearchRequestExample()
        {
            var jsonResponse = await SavedSearchJson.GetSavedSearches();
            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForTimelineRequestExample()
        {
            var jsonResponse = await TimelineJson.GetHomeTimeline(2);
            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForTrendsRequestExample()
        {
            var jsonResponse = await TrendsJson.GetTrendsAt(1);
            Console.WriteLine(jsonResponse);
        }

        public static async Task Json_GetJsonForTweetRequestExample()
        {
            var result = await Client.RequestExecutor.Tweets.GetTweet(42);
            Console.WriteLine(result.Json);
        }

        #endregion

        #region Exception

        public static async Task Exceptions_GetExceptionsInfo()
        {
            Auth.Credentials = null;

            // default
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();

            if (authenticatedUser == null)
            {
                var lastException = ExceptionHandler.GetLastException();
                Console.WriteLine(lastException.TwitterDescription);
            }

            // throw exception
            ExceptionHandler.SwallowWebExceptions = false;

            try
            {
                // ReSharper disable once RedundantAssignment
                authenticatedUser = await Client.Account.GetAuthenticatedUser();
            }
            catch (TwitterException ex)
            {
                Console.WriteLine(ex.TwitterDescription);
            }
        }

        #endregion

        #region Manual Query

        public static async Task ManualQuery_Example()
        {
            const string getHomeTimelineQuery = "https://api.twitter.com/1.1/statuses/home_timeline.json";

            // Execute Query can either return a json or a DTO interface
            var tweetsDTO = await TwitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(getHomeTimelineQuery);
            tweetsDTO.ForEach(tweetDTO => Console.WriteLine(tweetDTO.Text));
        }

        #endregion

        #region Configuration (Timeout/Proxy)

        public static void ConfigureTweetinvi()
        {
            if (!ExecuteExamples)
            {
                return;
            }

            TweetinviConfig.CurrentThreadSettings.ProxyConfig = new ProxyConfig("http://228.23.13.21:4287");

            // Configure a proxy with Proxy with username and password
            TweetinviConfig.CurrentThreadSettings.ProxyConfig = new ProxyConfig("http://228.23.13.21:4287", new NetworkCredential("username", "password"));

            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 5000;
            TweetinviConfig.CurrentThreadSettings.UploadTimeout = 90000;
        }

        public static void GlobalEvents()
        {
            if (!ExecuteExamples)
            {
                return;
            }

            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} is about to be executed.", args.TwitterQuery);
            };

            TweetinviEvents.QueryAfterExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} has just been executed.", args.TwitterQuery);
            };

            TweetinviEvents.CurrentThreadEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} is about to be executed in the main Thread.", args.TwitterQuery);
            };
        }

        #endregion

        #region Help

        public static async Task GetTwitterPrivacyPolicy()
        {
            var privacyPolicy = await Help.GetTwitterPrivacyPolicy();
            Console.WriteLine(privacyPolicy);
        }

        #endregion
    }
}