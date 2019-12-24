using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xUnitinvi.TestHelpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TestPriorityAttribute : Attribute
    {
        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }

    public class XUnitPriorityOrder : ITestCollectionOrderer
    {
        static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)  where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            result = new TValue();
            dictionary[key] = result;

            return result;
        }

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            var testCollectionsPerPriority = new SortedDictionary<int, List<ITestCollection>>();

            foreach (var testCollection in testCollections)
            {
                var priority = 0;

                foreach (var attribute in testCollection.CollectionDefinition.GetCustomAttributes((typeof(TestPriorityAttribute).AssemblyQualifiedName)))
                {
                    priority = attribute.GetNamedArgument<int>("Priority");
                }

                GetOrCreate(testCollectionsPerPriority, priority).Add(testCollection);
            }

            foreach (var testCollectionsForSpecificPriority in testCollectionsPerPriority.Keys.Select(priority => testCollectionsPerPriority[priority]))
            {
                testCollectionsForSpecificPriority.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.CollectionDefinition.Name, y.CollectionDefinition.Name));

                foreach (var testCollection in testCollectionsForSpecificPriority)
                {
                    yield return testCollection;
                }
            }
        }
    }
}