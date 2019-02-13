using System.Collections.Generic;

namespace Tweetinvi.Core.ExecutionContext
{
    public class CrossExecutionContextPreparer : ICrossExecutionContextPreparer
    {
        private readonly IEnumerable<ICrossExecutionContextPreparable> _executionContextInstances;

        public CrossExecutionContextPreparer(IEnumerable<ICrossExecutionContextPreparable> executionContextInstances)
        {
            _executionContextInstances = executionContextInstances;
        }

        public void Prepare()
        {
            foreach (ICrossExecutionContextPreparable executionContextInstance in _executionContextInstances)
            {
                executionContextInstance.InitializeExecutionContext();
            }
        }
    }
}
