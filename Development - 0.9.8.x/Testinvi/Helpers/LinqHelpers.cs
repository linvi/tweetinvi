using System.Collections.Generic;
using System.Linq;

namespace Testinvi.Helpers
{
    public static class LinqHelpers
    {
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            var sourceContainsAllTargets = !source.Except(target).Any();
            var targetContainsAllSources = !target.Except(source).Any();

            return sourceContainsAllTargets && targetContainsAllSources;
        }
    }
}