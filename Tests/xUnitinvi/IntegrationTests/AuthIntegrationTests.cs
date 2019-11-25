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
using Tweetinvi.Parameters.Auth;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.IntegrationTests
{
    public class AuthIntegrationTests
    {
        private readonly ITestOutputHelper _logger;

        public AuthIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllAuthTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            _logger.WriteLine($"Starting {nameof(BearerToken)}");
            await BearerToken().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(BearerToken)} succeeded");

            _logger.WriteLine($"Starting {nameof(AuthenticateWithPinCode)}");
            await AuthenticateWithPinCode().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(AuthenticateWithPinCode)} succeeded");

            _logger.WriteLine($"Starting {nameof(AuthenticateWithRedirectUrl)}");
            await AuthenticateWithRedirectUrl().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(AuthenticateWithRedirectUrl)} succeeded");
        }

        [Fact]
        public async Task BearerToken()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var testCreds = IntegrationTestConfig.TweetinviTest.Credentials;
            var appCreds = new TwitterCredentials(testCreds.ConsumerKey, testCreds.ConsumerSecret);

            var appClient = new TwitterClient(appCreds);
            await appClient.Auth.InitializeClientBearerToken().ConfigureAwait(false);

            var tweet = await appClient.Tweets.GetTweet(979753598446948353).ConfigureAwait(false);

            // assert
            Assert.Matches("Tweetinvi 3.0", tweet.Text);
        }

        [Fact]
        public async Task AuthenticateWithPinCode()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            // act
            var authenticationClient = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials);
            var authenticationContext = await authenticationClient.Auth.StartAuthProcess().ConfigureAwait(false);
            var authUrl = authenticationContext.AuthorizationURL;

            // ask the user for the pin code
            var pinCode = await ExtractPinCodeFromTwitterAuthPage(authUrl).ConfigureAwait(false);

            var userCredentials = await AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext).ConfigureAwait(false);
            var authenticatedClient = new TwitterClient(userCredentials);
            var authenticatedUser = await authenticatedClient.Account.GetAuthenticatedUser().ConfigureAwait(false);

            // assert
            Assert.Equal(authenticatedUser.ScreenName, IntegrationTestConfig.ProtectedUser.AccountId);
        }

        [Fact]
        public async Task AuthenticateWithRedirectUrl()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var client = new TwitterClient(IntegrationTestConfig.TweetinviApi.Credentials);

            // The url used below has to be set in apps.twitter.com -> Callback Url
            var authContext = await client.Auth.StartAuthProcess(new StartUrlAuthProcessParameters("http://localhost:8042")
            {
                AuthAccessType = AuthAccessType.ReadWrite
            });

            var authenticatedClient = await GetAuthenticatedTwitterClientViaRedirect(authContext);

            // assert
            var authenticatedUser = await authenticatedClient.Account.GetAuthenticatedUser().ConfigureAwait(false);

            // has write permissions
            var tweet = await authenticatedClient.Tweets.PublishTweet("random tweet");
            await tweet.Destroy();

            Assert.Equal(authenticatedUser.ScreenName, IntegrationTestConfig.ProtectedUser.AccountId);
        }


        [Fact]
        public async Task AuthenticateWithReadOnlyPermissions()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var client = new TwitterClient(IntegrationTestConfig.TweetinviApi.Credentials);

            // The url used below has to be set in apps.twitter.com -> Callback Url
            var authContext = await client.Auth.StartAuthProcess(new StartUrlAuthProcessParameters("http://localhost:8042")
            {
                AuthAccessType = AuthAccessType.Read
            });

            var authenticatedClient = await GetAuthenticatedTwitterClientViaRedirect(authContext);
            var authenticatedUser = await authenticatedClient.Account.GetAuthenticatedUser().ConfigureAwait(false);

            // assert
            await Assert.ThrowsAsync<TwitterException>(() => authenticatedClient.Tweets.PublishTweet("random tweet"));

            Assert.Equal(authenticatedUser.ScreenName, IntegrationTestConfig.ProtectedUser.AccountId);
        }

        private async Task<TwitterClient> GetAuthenticatedTwitterClientViaRedirect(IAuthenticationContext authContext)
        {
            var expectAuthRequestTask = AExtensions.HttpRequest(new AssertHttpRequestConfig(_logger.WriteLine))
                .OnPort(8042)
                .WithATimeoutOf(TimeSpan.FromSeconds(30))
                .Matching(request => { return request.Url.AbsoluteUri.Contains(authContext.Token.AuthorizationKey); })
                .MustHaveHappened();

            await AuthenticateWithRedirectUrlOnTwitterAuthPage(authContext.AuthorizationURL, authContext.Token.AuthorizationKey).ConfigureAwait(false);

            var authRequest = await expectAuthRequestTask.ConfigureAwait(false);

            // Ask the user to enter the pin code given by Twitter
            var callbackUrl = authRequest.Url.AbsoluteUri;
            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = await AuthFlow.CreateCredentialsFromCallbackURL(callbackUrl, authContext).ConfigureAwait(false);
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
            usernameTextField.SendKeys(IntegrationTestConfig.ProtectedUser.AccountId);

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