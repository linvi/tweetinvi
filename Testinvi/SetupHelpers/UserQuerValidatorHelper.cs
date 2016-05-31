using System;
using FakeItEasy;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.SetupHelpers
{
    public static class UserQuerValidatorHelper
    {
        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, bool? result = null)
        {
          
        }

        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, string screenName, bool result)
        {
           
        }

        public static void ArrangeIsUserIdValid(this Fake<IUserQueryValidator> userQueryValidator, bool? result = null)
        {
        
        }

        public static void ArrangeCanUserBeIdentified(this Fake<IUserQueryValidator> userQueryValidator, IUserIdentifier userIdentifier, bool result)
        {
          
        }
    }
}