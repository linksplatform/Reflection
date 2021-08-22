using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the method info extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// <para>
        /// Gets the il bytes using the specified method info.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="methodInfo">
        /// <para>The method info.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The byte array</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetILBytes(this MethodInfo methodInfo) => methodInfo.GetMethodBody().GetILAsByteArray();

        /// <summary>
        /// <para>
        /// Gets the parameter types using the specified method info.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="methodInfo">
        /// <para>The method info.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type array</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetParameterTypes(this MethodInfo methodInfo) => methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
    }
}
