using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.ExecutionContext
{
    public interface ICrossExecutionContextPreparer
    {
        /// <summary>
        /// Prepare the Execution Context for copying.
        /// Any objects in the Execution Context whose (modified) values we want to be available outside of the copied
        /// context need to be instantiated before copying.
        /// </summary>
        void Prepare();
    }
}
