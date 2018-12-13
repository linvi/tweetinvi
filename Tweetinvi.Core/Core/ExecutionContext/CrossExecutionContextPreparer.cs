using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.ExecutionContext
{
    public class CrossExecutionContextPreparer : ICrossExecutionContextPreparer
    {
        private readonly IEnumerable<ICrossExecutionContextPreparable> _preparableObjects;

        public CrossExecutionContextPreparer(IEnumerable<ICrossExecutionContextPreparable> preparableObjects)
        {
            _preparableObjects = preparableObjects;
        }

        public void Prepare()
        {
            foreach (ICrossExecutionContextPreparable preparableObject in _preparableObjects)
            {
                preparableObject.PrepareExecutionContext();
            }
        }
    }
}
