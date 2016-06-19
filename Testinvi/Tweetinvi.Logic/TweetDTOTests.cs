using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Logic;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Wrapper;
using Tweetinvi.WebLogic;

namespace Testinvi.Tweetinvi.Logic
{
    [TestClass]
    public class TweetDTOTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var userFactory = A.Fake<IUserFactory>();
            userFactory.CallsTo(x => x.GenerateUserFromDTO(It.IsAny<IUserDTO>())).Returns(null);

            var jsonConverterLib = new JsonConvertWrapper();
            var jsonConverter = new JsonObjectConverter(jsonConverterLib);

            var tweetDTO = jsonConverter.DeserializeObject<ITweetDTO>(_extendedTweet);
            var tweet = new Tweet(tweetDTO, null, null, userFactory, null);

            Assert.AreEqual(tweet.Text, "Peek-a-boo! https://t.co/R3P6waHxRa");
        }

        static string _extendedTweet = @"{
          'created_at': 'Mon Mar 07 15:13:47 +0000 2016',
          'id': 706860403981099008,
          'id_str': '706860403981099008',
          'full_text': 'Peek-a-boo! https://t.co/R3P6waHxRa',
          'display_text_range': [
            0,
            35
          ],
          'entities': {
            'hashtags': [],
            'symbols': [],
            'user_mentions': [],
            'urls': [],
            'media': [
              {
                'id': 706860403746181121,
                'id_str': '706860403746181121',
                'indices': [
                  12,
                  35
                ],
                'media_url': 'http://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'media_url_https': 'https://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'url': 'https://t.co/R3P6waHxRa',
                'display_url': 'pic.twitter.com/R3P6waHxRa',
                'expanded_url': 'http://twitter.com/jeremycloud/status/706860403981099008/photo/1',
                'type': 'photo',
                'sizes': {
                  'medium': {
                    'w': 600,
                    'h': 398,
                    'resize': 'fit'
                  },
                  'small': {
                    'w': 340,
                    'h': 226,
                    'resize': 'fit'
                  },
                  'thumb': {
                    'w': 150,
                    'h': 150,
                    'resize': 'crop'
                  },
                  'large': {
                    'w': 1024,
                    'h': 680,
                    'resize': 'fit'
                  }
                }
              }
            ]
          },
          'extended_entities': {
            'media': [
              {
                'id': 706860403746181121,
                'id_str': '706860403746181121',
                'indices': [
                  12,
                  35
                ],
                'media_url': 'http://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'media_url_https': 'https://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'url': 'https://t.co/R3P6waHxRa',
                'display_url': 'pic.twitter.com/R3P6waHxRa',
                'expanded_url': 'http://twitter.com/jeremycloud/status/706860403981099008/photo/1',
                'type': 'photo',
                'sizes': {
                  'medium': {
                    'w': 600,
                    'h': 398,
                    'resize': 'fit'
                  },
                  'small': {
                    'w': 340,
                    'h': 226,
                    'resize': 'fit'
                  },
                  'thumb': {
                    'w': 150,
                    'h': 150,
                    'resize': 'crop'
                  },
                  'large': {
                    'w': 1024,
                    'h': 680,
                    'resize': 'fit'
                  }
                }
              }
            ]
          },
          'truncated': false,
          'source': '<a href=\'http://www.apple.com/\' rel=\'nofollow\'>OS X</a>',
          'in_reply_to_status_id': null,
          'in_reply_to_status_id_str': null,
          'in_reply_to_user_id': null,
          'in_reply_to_user_id_str': null,
          'in_reply_to_screen_name': null,
          'user': {
            'id': 15062340,
            'id_str': '15062340',
            'name': '/dev/cloud/jeremy',
            'screen_name': 'jeremycloud',
            'location': 'Madison, Wisconsin',
            'description': 'Professional yak shaver. Amateur bike shedder.',
            'url': 'https://t.co/FcYeBkOpVY',
            'entities': {
              'url': {
                'urls': [
                  {
                    'url': 'https://t.co/FcYeBkOpVY',
                    'expanded_url': 'http://about.me/jeremy.cloud',
                    'display_url': 'about.me/jeremy.cloud',
                    'indices': [
                      0,
                      23
                    ]
                  }
                ]
              },
              'description': {
                'urls': []
              }
            },
            'protected': false,
            'followers_count': 4324,
            'friends_count': 410,
            'listed_count': 103,
            'created_at': 'Mon Jun 09 17:00:58 +0000 2008',
            'favourites_count': 815,
            'utc_offset': -18000,
            'time_zone': 'Central Time (US & Canada)',
            'geo_enabled': true,
            'verified': false,
            'statuses_count': 2218,
            'lang': 'en',
            'contributors_enabled': false,
            'is_translator': false,
            'is_translation_enabled': false,
            'profile_background_color': '000000',
            'profile_background_image_url': 'http://abs.twimg.com/images/themes/theme1/bg.png',
            'profile_background_image_url_https': 'https://abs.twimg.com/images/themes/theme1/bg.png',
            'profile_background_tile': false,
            'profile_image_url': 'http://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
            'profile_image_url_https': 'https://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
            'profile_banner_url': 'https://pbs.twimg.com/profile_banners/15062340/1447451621',
            'profile_link_color': '4A913C',
            'profile_sidebar_border_color': '000000',
            'profile_sidebar_fill_color': '000000',
            'profile_text_color': '000000',
            'profile_use_background_image': false,
            'has_extended_profile': true,
            'default_profile': false,
            'default_profile_image': false,
            'following': true,
            'follow_request_sent': false,
            'notifications': false
          },
          'geo': null,
          'coordinates': null,
          'place': null,
          'contributors': null,
          'is_quote_status': false,
          'retweet_count': 0,
          'favorite_count': 8,
          'favorited': false,
          'retweeted': false,
          'possibly_sensitive': false,
          'possibly_sensitive_appealable': false,
          'lang': 'en'
        }";
    }
}
