using System;
using System.Collections.Generic;
using System.Linq;

namespace Tweetinvi.Core.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static bool ContainsSameObjectsAs<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
        {
            return ContainsSameObjectsAs(collection.ToArray(), collection2.ToArray());
        }

        public static bool ContainsSameObjectsAs<T>(this T[] collection, T[] collection2, bool enforceOrder = false)
        {
            // Small optimization compared to the IEnumerable version

            if (collection.Length != collection2.Length)
            {
                return false;
            }

            if (!enforceOrder)
            {
                return collection.Except(collection2).IsEmpty();
            }

            for (int i = 0; i < collection.Length; ++i)
            {
                if (!collection[i].Equals(collection2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ContainsSameObjectsAs<T>(this IList<T> collection, IList<T> collection2, bool enforceOrder = false) where T : IEquatable<T>
        {
            // Small optimization compared to the IEnumerable version
            if (collection.Count != collection2.Count)
            {
                return false;
            }

            if (!enforceOrder)
            {
                return collection.Except(collection2).IsEmpty();
            }
            else
            {
                for (int i = 0; i < collection.Count; ++i)
                {
                    if (!collection[i].Equals(collection2[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }

        public static TSource JustOneOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> isMatching) where TSource : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            TSource result = null;
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TSource current = enumerator.Current;

                    if (isMatching(current))
                    {
                        if (result == null)
                        {
                            result = current;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                return result;
            }
        }

        public static TSource JustOneOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0:
                        return default;
                    case 1:
                        return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return default;
                    }

                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return current;
                    }
                }
            }

            return default;
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> areTheSame) where TSource : class
        {
            var sourceArray = source.ToArray();
            var result = new List<TSource>();

            foreach (var element in sourceArray)
            {
                if (!result.Any(x => x == element || areTheSame(x, element)))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        public static IEnumerable<TSource> SafeConcat<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target, params IEnumerable<TSource>[] additionalTargets)
        {
            var result = new List<TSource>();

            if (source != null)
            {
                result.AddRange(source);
            }

            if (target != null)
            {
                result.AddRange(target);
            }

            if (additionalTargets != null)
            {
                foreach (var additionalTarget in additionalTargets.Where(x => x != null))
                {
                    result.AddRange(additionalTarget);
                }
            }

            return result;
        }

        public static void AddRangeSafely<TSource>(this List<TSource> source, IEnumerable<TSource> newElements)
        {
            if (newElements != null)
            {
                source.AddRange(newElements);
            }
        }
    }
}