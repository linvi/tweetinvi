using System;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    // The is the last collection to run as we do not want any of the access token/bearer to be invalidated
    // before running the other tests
    [Collection("EndToEndTests"), Order(11)]
    public class AuthEndToEndTests : TweetinviTest
    {
        // Fact order is done in such a way that AuthenticateWithPinCode runs last
        // So that we can get the new bearer credentials from its logs

        public AuthEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task BearerToken()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            var testCreds = EndToEndTestConfig.TweetinviTest.Credentials;
            var appCreds = new TwitterCredentials(testCreds.ConsumerKey, testCreds.ConsumerSecret);

            var appClient = new TwitterClient(appCreds);
            await appClient.Auth.InitializeClientBearerToken();

            var tweet = await appClient.Tweets.GetTweet(979753598446948353);

            // assert
            Assert.Matches("Tweetinvi 3.0", tweet.Text);
        }

        [Fact]
        public async Task InvalidateBearerToken()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            var accountCreds = EndToEndTestConfig.TweetinviTest.Credentials;
            var consumerCreds = new TwitterCredentials(accountCreds.ConsumerKey, accountCreds.ConsumerSecret);
            var client = new TwitterClient(consumerCreds);
            await client.Auth.InitializeClientBearerToken();
            var accountUser = await client.Users.GetUser(EndToEndTestConfig.TweetinviTest.AccountId);

            // act
            await client.Auth.InvalidateBearerToken();

            // assert
            Assert.Equal(accountUser.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            await Assert.ThrowsAsync<TwitterException>(() => client.Users.GetUser(EndToEndTestConfig.TweetinviTest.AccountId));
        }

        [Fact]
        public async Task InvalidateAccessToken()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            var authenticationClient = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials);
            var authenticationRequest = await authenticationClient.Auth.RequestAuthenticationUrl();
            var authUrl = authenticationRequest.AuthorizationURL;

            // ask the user for the pin code
            var verifierCode = await ExtractPinCodeFromTwitterAuthPage(authUrl);
            var userCredentials = await authenticationClient.Auth.RequestCredentialsFromVerifierCode(verifierCode, authenticationRequest);

            var client = new TwitterClient(userCredentials);
            var accountUser = await client.Users.GetAuthenticatedUser();

            // act
            await Task.Delay(TimeSpan.FromSeconds(3)); // giving time to Twitter to process the new credentials
            await client.Auth.InvalidateAccessToken();

            // assert
            Assert.Equal(accountUser.ScreenName, EndToEndTestConfig.ProtectedUser.AccountId);
            await Assert.ThrowsAsync<TwitterException>(() => client.Users.GetAuthenticatedUser());
        }

        [Fact, Order(10)]
        public async Task AuthenticateWithPinCode()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            // act
            var authenticationClient = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);
            var authenticationRequest = await authenticationClient.Auth.RequestAuthenticationUrl();
            var authUrl = authenticationRequest.AuthorizationURL;

            // ask the user for the pin code
            var verifierCode = await ExtractPinCodeFromTwitterAuthPage(authUrl);
            var userCredentials = await authenticationClient.Auth.RequestCredentialsFromVerifierCode(verifierCode, authenticationRequest);
            var authenticatedClient = new TwitterClient(userCredentials);
            var authenticatedUser = await authenticatedClient.Users.GetAuthenticatedUser();

            // assert
            Assert.Equal(authenticatedUser.ScreenName, EndToEndTestConfig.ProtectedUser.AccountId);

            _logger.WriteLine("public static readonly IntegrationTestAccount ProtectedUserAuthenticatedToTweetinviApi = new IntegrationTestAccount\n" +
            "{\n" +
                "\t\t\t// Careful as these credentials will be refreshed by AuthEndToEndTests\n" +
                "\t\t\t// Run AuthEndToEndTests.AuthenticateWithPinCode and copy paste output to replace here\n" +
                "Credentials = new TwitterCredentials(TweetinviApi.Credentials.ConsumerKey, TweetinviApi.Credentials.ConsumerSecret,\n" +
                $"\"{userCredentials.AccessToken}\", \"{userCredentials.AccessTokenSecret}\"),\n" +
                "AccountId = \"artwolkt\",\n" +
                "UserId = 42290825\n" +
            "};");
        }

        [Fact]
        public async Task AuthenticateWithRedirectUrl()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            var client = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);

            // The url used below has to be set in apps.twitter.com -> Callback Url
            var authContext = await client.Auth.RequestAuthenticationUrl(new RequestUrlAuthUrlParameters("http://localhost:8042")
            {
                AuthAccessType = AuthAccessType.ReadWrite
            });

            var authenticatedClient = await GetAuthenticatedTwitterClientViaRedirect(client, authContext);

            // assert
            var authenticatedUser = await authenticatedClient.Users.GetAuthenticatedUser();

            // has write permissions
            var tweet = await authenticatedClient.Tweets.PublishTweet("random tweet");
            await tweet.Destroy();

            Assert.Equal(authenticatedUser.ScreenName, EndToEndTestConfig.ProtectedUser.AccountId);
        }

        [Fact]
        public async Task AuthenticateWithReadOnlyPermissions()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunAuthTests)
                return;

            var client = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);

            // The url used below has to be set in apps.twitter.com -> Callback Url
            var authContext = await client.Auth.RequestAuthenticationUrl(new RequestUrlAuthUrlParameters("http://localhost:8042")
            {
                AuthAccessType = AuthAccessType.Read
            });

            var authenticatedClient = await GetAuthenticatedTwitterClientViaRedirect(client, authContext);
            var authenticatedUser = await authenticatedClient.Users.GetAuthenticatedUser();

            // assert
            await Assert.ThrowsAsync<TwitterException>(() => authenticatedClient.Tweets.PublishTweet("random tweet"));

            Assert.Equal(authenticatedUser.ScreenName, EndToEndTestConfig.ProtectedUser.AccountId);
        }

        private async Task<TwitterClient> GetAuthenticatedTwitterClientViaRedirect(ITwitterClient client, IAuthenticationRequest authRequest)
        {
            var expectAuthRequestTask = AExtensions.HttpRequest(new AssertHttpRequestConfig(_logger.WriteLine))
                .OnPort(8042)
                .WithATimeoutOf(TimeSpan.FromSeconds(30))
                .Matching(request => { return request.Url.AbsoluteUri.Contains(authRequest.AuthorizationKey); })
                .MustHaveHappened();

            await AuthenticateWithRedirectUrlOnTwitterAuthPage(authRequest.AuthorizationURL, authRequest.AuthorizationKey);

            var authHttpRequest = await expectAuthRequestTask;

            // Ask the user to enter the pin code given by Twitter
            var callbackUrl = authHttpRequest.Url.AbsoluteUri;

            var userCredentials = await client.Auth.RequestCredentialsFromCallbackUrl(callbackUrl, authRequest);
            var authenticatedClient = new TwitterClient(userCredentials);
            return authenticatedClient;
        }


        private Task<string> ExtractPinCodeFromTwitterAuthPage(string authUrl)
        {
            var geckoPath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, "../../../");
            var service = FirefoxDriverService.CreateDefaultService(geckoPath, "geckodriver");

            return Task.Factory.StartNew(() =>
            {
                using (var webDriver = new FirefoxDriver(service))
                {
                    AuthenticateOnTwitterWebsite(authUrl, webDriver);
                    return ExtractPinCodeFromTwitterWebsite(webDriver);
                }
            });
        }

        private Task AuthenticateWithRedirectUrlOnTwitterAuthPage(string authUrl, string expectedUrlContent)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var webDriver = CreateDriver())
                {
                    AuthenticateOnTwitterWebsite(authUrl, webDriver);

                    _logger.WriteLine($"{DateTime.Now.ToLongTimeString()} - waiting for httpExpect...");
                    var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10))
                    {
                        PollingInterval = TimeSpan.FromSeconds(1)
                    };

                    wait.Until(driver => { return driver.PageSource.Contains("HttpExpect") && driver.Url.Contains(expectedUrlContent); });
                    _logger.WriteLine($"{DateTime.Now.ToLongTimeString()} - wait completed!");
                }
            });
        }

        private void AuthenticateOnTwitterWebsite(string authUrl, RemoteWebDriver webDriver)
        {
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            webDriver.Url = authUrl;

            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));

            var usernameTextField = webDriver.FindElementById("username_or_email");
            usernameTextField.SendKeys(EndToEndTestConfig.ProtectedUser.AccountId);

            // ReSharper disable once CC0021
            var passwordTextField = webDriver.FindElementById("password");
            passwordTextField.SendKeys(Environment.GetEnvironmentVariable("TWEETINVI_PASS"));

            passwordTextField.Submit();

            _logger.WriteLine($"{DateTime.Now.ToLongTimeString()} - authentication credentials submitted");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
            _logger.WriteLine($"{DateTime.Now.ToLongTimeString()} - authentication successfully moved to next page");

            var emailTextFields = webDriver.FindElementsByClassName("js-username-field");
            var isTwitterPromptingForSecondAuthentication = emailTextFields.Count == 1;

            if (isTwitterPromptingForSecondAuthentication)
            {
                var secondPasswordTextField = webDriver.FindElementByClassName("js-password-field");
                emailTextFields[0].SendKeys(Environment.GetEnvironmentVariable("TWEETINVI_EMAIL"));
                secondPasswordTextField.SendKeys(Environment.GetEnvironmentVariable("TWEETINVI_PASS"));
                secondPasswordTextField.Submit();
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
            }

            _logger.WriteLine($"{DateTime.Now.ToLongTimeString()} - authentication completed");
        }

        private static RemoteWebDriver CreateDriver()
        {
            var geckoPath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, "../../../");
            var service = FirefoxDriverService.CreateDefaultService(geckoPath, "geckodriver");

            return new FirefoxDriver(service);
        }

        private static string ExtractPinCodeFromTwitterWebsite(IWebDriver webDriver)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromSeconds(1)
            };

            wait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(By.CssSelector("#oauth_pin code"));
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            var pinCodeTextField = webDriver.FindElement(By.CssSelector("#oauth_pin code"));
            var pinCode = pinCodeTextField.Text;

            return pinCode;
        }
    }
}