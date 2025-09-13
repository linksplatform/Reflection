using System.Reflection;
using System.Runtime.CompilerServices;


namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the field info extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// <para>
        /// Gets the static value using the specified field info.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="fieldInfo">
        /// <para>The field info.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticValue<T>(this FieldInfo fieldInfo) => (T)fieldInfo.GetValue(null);
    }
}
