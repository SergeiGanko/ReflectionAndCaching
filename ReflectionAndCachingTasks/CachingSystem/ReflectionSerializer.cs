using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CachingSystem
{
    /// <summary>
    /// ReflectionSerializer class
    /// </summary>
    public static class ReflectionSerializer
    {
        /// <summary>
        /// Serializes the specified object to json-like string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>Returns string</returns>
        public static string Serialize<T>(T obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = new StringBuilder();
            builder.AppendLine("{");

            foreach (PropertyInfo property in properties)
            {
                builder.Append($"\"{property.Name}\":\"{GetString(obj, property)}\",");
            }

            builder.AppendLine("}");

            return builder.ToString();
        }

        /// <summary>
        /// Deserializes the specified  json-like string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var obj = Activator.CreateInstance<T>();

            var keyValuePairs = json.Split(',');
            var charsToTrim = new[] { '{', '\r', '\n', '}' };

            var list = new List<string>();

            foreach (var pair in keyValuePairs)
            {
                if (pair.Contains(':'))
                {
                    list.Add(pair.Replace("\"", string.Empty).Trim(charsToTrim));
                }
            }

            for (int i = 0; i < properties.Count(); i++)
            {
                var pair = list[i].Split(':');
                properties[i].SetValue(obj, Convert.ChangeType(pair[1], properties[i].PropertyType));
            }

            return obj;
        }

        /// <summary>
        /// Gets the expiration time from model using reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <returns>Returns Timespan</returns>
        public static TimeSpan GetExpirationTime<T>(T model)
        {
            Type type = typeof(T);
            var cacheExpirationTimerAttribute = (CacheExpirationTimerAttribute)type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType() == typeof(CacheExpirationTimerAttribute));
            if (!ReferenceEquals(cacheExpirationTimerAttribute, null))
            {
                return TimeSpan.FromSeconds(cacheExpirationTimerAttribute.Seconds);
            }

            return TimeSpan.Zero;
        }

        /// <summary>
        /// Gets the string representation of property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns>Returns object</returns>
        private static object GetString<T>(T obj, PropertyInfo property) => property.GetValue(obj) ?? String.Empty;
    }
}
