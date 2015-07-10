using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFramework.SoftDeletable {
    internal static class PropertyReflection {
        public static Action<TType, TProperty> GetValueSetter<TType, TProperty>(PropertyInfo propertyInfo) {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            var instance = Expression.Parameter(typeof(TType));
            var argument = Expression.Parameter(typeof(TProperty));
            var setterCall = Expression.Call(
                Expression.Convert(instance, propertyInfo.DeclaringType),
                propertyInfo.GetSetMethod(true),
                Expression.Convert(argument, propertyInfo.PropertyType)
                );
            return Expression.Lambda<Action<TType, TProperty>>(setterCall, instance, argument).Compile();
        }
    }
}
