using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.Exceptions
{
    public interface IExceptionHandlerFactory
    {
        IExceptionHandler Create();
    }
}
