using System;
using System.Linq;
// ReSharper disable once RedundantUsingDirective -- dotnet cannot compile
using System.Reflection;

namespace Tweetinvi.Core.Helpers
{
    public static class ResourcesHelper
    {
        public static string GetResourceByName(object source, string resourceName)
        {
            var type = source.GetType();

            return GetResourceByType(type, resourceName, source);
        }

        public static string GetResourceByType(Type type, string resourceName, object source = null)
        {
            var fields = type.GetFields().ToDictionary(x => x.Name);

            if (fields.TryGetValue(resourceName, out var fieldInfo))
            {
                var value = fieldInfo.GetValue(source);
                return value as string;
            }

            var properties = type.GetProperties().ToDictionary(x => x.Name);

            if (properties.TryGetValue(resourceName, out var propertyInfo))
            {
                var value = propertyInfo.GetValue(source, new object[0]);
                return value as string;
            }

            return null;
        }
    }
}