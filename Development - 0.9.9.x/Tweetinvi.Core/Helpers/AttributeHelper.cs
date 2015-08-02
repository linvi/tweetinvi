using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Core.Helpers
{
    public interface IAttributeHelper
    {
        IEnumerable<T1> GetAttributes<T1>(MethodInfo method);
        IEnumerable<T1> GetAttributes<T1>(MemberInfo property);
        Dictionary<T2, PropertyInfo> GetAllPropertiesAttributes<T1, T2>() where T2 : Attribute;
        IEnumerable<T1> GetPropertyAttributes<T1, T2, T3>(Expression<Func<T3>> propertyExpression) where T1 : Attribute;
    }

    public class AttributeHelper : IAttributeHelper
    {
        public Dictionary<T2, PropertyInfo> GetAllPropertiesAttributes<T1, T2>() where T2 : Attribute
        {
            var propertiesAttributes = new Dictionary<T2, PropertyInfo>();
            var properties = typeof(T1).GetProperties();

            foreach (var property in properties)
            {
                var customAttributes = property.GetCustomAttributes(typeof(T2), false).OfType<T2>();
                var customAttribute = customAttributes.JustOneOrDefault();

                if (customAttribute != null)
                {
                    propertiesAttributes.Add(customAttribute, property);
                }
            }

            return propertiesAttributes;
        }

        public IEnumerable<T1> GetAttributes<T1>(MethodInfo method)
        {
            return  method.GetCustomAttributes(typeof (T1), false).OfType<T1>();
        }

        public IEnumerable<T1> GetAttributes<T1>(MemberInfo property)
        {
            return property.GetCustomAttributes(typeof(T1), false).OfType<T1>();
        }

        public IEnumerable<T1> GetPropertyAttributes<T1, T2, T3>(Expression<Func<T3>> propertyExpression) where T1 : Attribute
        {
            if (propertyExpression == null)
            {
                return null;
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body == null || body.Member == null)
            {
                return null;
            }

            var memberName = body.Member.Name;
            var properties = typeof (T2).GetProperties();
            var property = properties.FirstOrDefault(x => x.Name == memberName);

            if (property == null)
            {
                return null;
            }

            var customAttributes = property.GetCustomAttributes(typeof (T1), false).OfType<T1>();
            return customAttributes;
        }
    }
}