﻿using Tweetinvi.Core;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class Mention : Tweet, IMention
    {
        public Mention(
            ITweetDTO tweetDTO,
            ITweetController tweetController,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITaskFactory taskFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor) 
                
                : base(tweetDTO,
                       tweetController,
                       tweetFactory,
                       userFactory,
                       taskFactory,
                       tweetinviSettingsAccessor)
        {
            // Default constructor inheriting from the default Tweet constructor
        }

        public string Annotations { get; set; }
    }
}