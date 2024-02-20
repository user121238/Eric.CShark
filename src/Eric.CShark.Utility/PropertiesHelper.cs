using System;
using System.Reflection;

namespace Eric.CShark.Utility
{
    public static class PropertiesHelper
    {
        /// <summary>
        /// 重置类的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要重置的类</param>
        /// <param name="excludePredicate">排除不重置的属性</param>
        /// <param name="customSettings">设置特定属性</param>
        public static void ResetObjectPropertiesToDefault<T>(T obj, Func<PropertyInfo, bool> excludePredicate, Action<T>? customSettings = null) where T : class
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if ((excludePredicate != null && excludePredicate(propertyInfo)) || !propertyInfo.CanWrite)
                {
                    continue;
                }
                var defaultValue = propertyInfo.PropertyType.IsValueType
                    ? Activator.CreateInstance(propertyInfo.PropertyType)
                    : null;
                propertyInfo.SetValue(obj, defaultValue);
            }

            customSettings?.Invoke(obj);
        }
    }
}
