using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionTask
{
    /// <summary>
    /// Represents ReflectionEqualityComparer class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
    public class ReflectionEqualityComparer<T> : IEqualityComparer<T>
    {
        #region Public Members

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="firstObject">The first object.</param>
        /// <param name="secondObject">The second object.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public bool Equals(T firstObject, T secondObject)
        {
            return AreObjectsEqual(firstObject, secondObject);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(T obj)
        {
            int hash = 73;

            Type type = obj.GetType();

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo propertyInfo in properties)
            {
                hash = hash * 11 + propertyInfo.GetValue(obj, null).GetHashCode();
            }

            return hash;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="firstObject">The first object.</param>
        /// <param name="secondObject">The second object.</param>
        /// <returns>
        ///   <c>true</c> if firstObject and secondObject are equal; otherwise, <c>false</c>.
        /// </returns>
        private bool AreObjectsEqual(T firstObject, T secondObject)
        {
            if (ReferenceEquals(firstObject, secondObject))
            {
                return true;
            }

            if (ReferenceEquals(firstObject, null) || ReferenceEquals(secondObject, null))
            {
                return false;
            }

            Type type = typeof(T);

            if (typeof(IEquatable<T>).IsAssignableFrom(type) || type.IsValueType)
            {
                return EqualityComparer<T>.Default.Equals(firstObject, secondObject);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return CompareEnumerables(firstObject, secondObject);
            }

            if (type.IsClass || type.IsInterface)
            {
                return ComparePublicProperties(firstObject, secondObject);
            }

            return firstObject.Equals(secondObject);
        }

        /// <summary>
        /// Compares the public properties of two objects.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="firstObject">The first object.</param>
        /// <param name="secondObject">The second object.</param>
        /// <returns>True - if all properties of the first object equal to properties of the second object, otherwise - false</returns>
        private bool ComparePublicProperties<U>(U firstObject, U secondObject)
        {
            var properties = firstObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties)
            {
                var firstValue = propertyInfo.GetValue(firstObject);
                var secondValue = propertyInfo.GetValue(secondObject);

                if (IsPrimitiveOrValueType(propertyInfo.PropertyType))
                {
                    if (!firstValue.Equals(secondValue))
                    {
                        return false;
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    if (!CompareEnumerables(firstValue, secondValue))
                    {
                        return false;
                    }
                }
                else if (propertyInfo.PropertyType.IsClass ||
                         propertyInfo.PropertyType.IsInterface)
                {
                    if (!ComparePublicProperties(firstValue, secondValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified type is primitive type, string or value type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is primitive type, string or value type; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPrimitiveOrValueType(Type type)
        {
            if (type.IsPrimitive || type == typeof(string) || type.IsValueType || type == typeof(object))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares the enumerables.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>
        ///     <c>true</c> if the specified enumerables are equal; otherwise, <c>false</c>.
        /// </returns>
        private bool CompareEnumerables<U>(U first, U second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            var firstSequence = ((IEnumerable)first).Cast<object>();
            var secondSequence = ((IEnumerable)second).Cast<object>();

            if (firstSequence.Count() != secondSequence.Count())
            {
                return false;
            }

            var firstEnumerator = firstSequence.GetEnumerator();
            var secondEnumerator = secondSequence.GetEnumerator();

            for (int i = 0; i < firstSequence.Count(); i++)
            {
                firstEnumerator.MoveNext();
                secondEnumerator.MoveNext();

                Type type = firstEnumerator.Current.GetType();

                if (typeof(IEquatable<U>).IsAssignableFrom(type) || type.IsValueType)
                {
                    if (!firstEnumerator.Current.Equals(secondEnumerator.Current))
                    {
                        return false;
                    }
                }
                else
                {
                    return ComparePublicProperties(firstEnumerator.Current, secondEnumerator.Current);
                }
            }

            return true;
        }

        #endregion
    }
}
