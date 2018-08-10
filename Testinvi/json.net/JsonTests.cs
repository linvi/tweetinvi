namespace Testinvi.json.net
{
    public static class JsonTests
    {
        public static string TWEET_TEST_JSON = @"
        {
          ""created_at"": ""Thu Apr 06 15:24:15 +0000 2017"",
          ""id_str"": ""850006245121695744"",
          ""text"": ""1\/ Today we\u2019re sharing our vision for the future of the Twitter API platform!\nhttps:\/\/t.co\/XweGngmxlP"",
          ""user"": {
            ""id"": 42,
            ""name"": ""Twitter Dev"",
            ""screen_name"": ""TwitterDev"",
            ""location"": ""Internet"",
            ""url"": ""https:\/\/dev.twitter.com\/"",
            ""description"": ""Your official source for Twitter Platform news, updates & events. Need technical help? Visit https:\/\/twittercommunity.com\/ \u2328\ufe0f #TapIntoTwitter""
          },
          ""place"": {   
          },
          ""entities"": {
            ""hashtags"": [      
            ],
            ""urls"": [
              {
                ""url"": ""https:\/\/t.co\/XweGngmxlP"",
                ""unwound"": {
                  ""url"": ""https:\/\/cards.twitter.com\/cards\/18ce53wgo4h\/3xo1c"",
                  ""title"": ""Building the Future of the Twitter API Platform""
                }
              }
            ],
            ""user_mentions"": [     
            ]
          }
        }";

        public static string USER_TEST_JSON(long id)
        {
            return @"
            {
                ""id"": " + id + @",
                ""name"": ""Uesr Test"",
                ""screen_name"": ""UserTest"",
                ""location"": ""Internet"",
                ""url"": ""https:\/\/dev.twitter.com\/"",
                ""description"": ""UserTestDescription""
            }";
        }
    }
}
