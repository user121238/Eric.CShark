using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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




        public static T DeepCloneWithJsonSerialize<T>(this T obj) where T : class
        {
            if (obj is null)
            {
                throw new ArgumentNullException($"{nameof(obj)} is null");
            }
            var json = JsonSerializer.Serialize(obj);
            var result = JsonSerializer.Deserialize<T>(json);
            if (result is null)
            {
                throw new InvalidCastException();
            }
            return result;
        }


        public static T DeepCloneWithBinarySerialize<T>(this T obj) where T : class
        {
            var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            var result = (T)formatter.Deserialize(ms);
            ms.Close();
            return result;
        }


    }
}
