using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Platform.Collections.Lists;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1819 // Properties should not return arrays

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the types.
    /// </para>
    /// <para></para>
    /// </summary>
    public abstract class Types
    {
        /// <summary>
        /// <para>
        /// Gets the collection value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static ReadOnlyCollection<Type> Collection { get; } = new ReadOnlyCollection<Type>(System.Array.Empty<Type>());
        /// <summary>
        /// <para>
        /// Gets the array value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static Type[] Array => Collection.ToArray();

        /// <summary>
        /// <para>
        /// Returns the read only collection.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>A read only collection of type</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ReadOnlyCollection<Type> ToReadOnlyCollection()
        {
            var types = GetType().GetGenericArguments();
            var result = new List<Type>();
            AppendTypes(result, types);
            return new ReadOnlyCollection<Type>(result);
        }

        /// <summary>
        /// <para>
        /// Appends the types using the specified container.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="container">
        /// <para>The container.</para>
        /// <para></para>
        /// </param>
        /// <param name="types">
        /// <para>The types.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendTypes(List<Type> container, IList<Type> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                var element = types[i];
                if (element != typeof(Types))
                {
                    if (element.IsSubclassOf(typeof(Types)))
                    {
                        AppendTypes(container, element.GetStaticPropertyValue<ReadOnlyCollection<Type>>(nameof(Types<object>.Collection)));
                    }
                    else
                    {
                        container.Add(element);
                    }
                }
            }
        }
    }
}
