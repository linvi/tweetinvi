using FakeItEasy;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Testinvi.SetupHelpers
{
    public static class UserQueryValidatorHelper
    {
        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, bool? result = null)
        {
          
        }

        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, string screenName, bool result)
        {
           
        }

        public static void ArrangeIsUserIdValid(this IUserQueryValidator userQueryValidator, bool? result = null)
        {
        
        }

        public static void ArrangeCanUserBeIdentified(this IUserQueryValidator userQueryValidator, IUserIdentifier user, bool result)
        {
          
        }
    }
}