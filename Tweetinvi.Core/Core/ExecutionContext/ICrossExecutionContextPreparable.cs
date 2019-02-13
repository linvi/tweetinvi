using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.ExecutionContext
{
    public interface ICrossExecutionContextPreparable
    {
        /// <summary>
        /// Prepare the current execution context to be copied.
        /// If anything within the implementing class must exist in the parent context (so that the pointer to the object
        /// on the heap is cross execution context), they should be set in this method.
        /// </summary>
        void InitializeExecutionContext();
    }
}
