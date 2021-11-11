using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Platform.Exceptions;

// ReSharper disable AssignmentInConditionalExpression
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable StaticFieldInGenericType
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the numeric type.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class NumericType<T>
    {
        /// <summary>
        /// <para>
        /// The type.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly Type Type;
        /// <summary>
        /// <para>
        /// The underlying type.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly Type UnderlyingType;
        /// <summary>
        /// <para>
        /// The signed version.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly Type SignedVersion;
        /// <summary>
        /// <para>
        /// The unsigned version.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly Type UnsignedVersion;
        /// <summary>
        /// <para>
        /// The is float point.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly bool IsFloatPoint;
        /// <summary>
        /// <para>
        /// The is numeric.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly bool IsNumeric;
        /// <summary>
        /// <para>
        /// The is signed.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly bool IsSigned;
        /// <summary>
        /// <para>
        /// The can be numeric.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly bool CanBeNumeric;
        /// <summary>
        /// <para>
        /// The is nullable.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly bool IsNullable;
        /// <summary>
        /// <para>
        /// The bytes size.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly int BytesSize;
        /// <summary>
        /// <para>
        /// The bits size.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly int BitsSize;
        /// <summary>
        /// <para>
        /// The min value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly T MinValue;
        /// <summary>
        /// <para>
        /// The max value.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly T MaxValue;

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="NumericType"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static NumericType()
        {
            try
            {
                var type = typeof(T);
                var isNullable = type.IsNullable();
                var underlyingType = isNullable ? Nullable.GetUnderlyingType(type) : type;
                var canBeNumeric = underlyingType.CanBeNumeric();
                var isNumeric = underlyingType.IsNumeric();
                var isSigned = underlyingType.IsSigned();
                var isFloatPoint = underlyingType.IsFloatPoint();
                var bytesSize = Marshal.SizeOf(underlyingType);
                var bitsSize = bytesSize * 8;
                GetMinAndMaxValues(underlyingType, out T minValue, out T maxValue);
                GetSignedAndUnsignedVersions(underlyingType, isSigned, out Type signedVersion, out Type unsignedVersion);
                Type = type;
                IsNullable = isNullable;
                UnderlyingType = underlyingType;
                CanBeNumeric = canBeNumeric;
                IsNumeric = isNumeric;
                IsSigned = isSigned;
                IsFloatPoint = isFloatPoint;
                BytesSize = bytesSize;
                BitsSize = bitsSize;
                MinValue = minValue;
                MaxValue = maxValue;
                SignedVersion = signedVersion;
                UnsignedVersion = unsignedVersion;
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
        }
[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetMinAndMaxValues(Type type, out T minValue, out T maxValue)
        {
            if (type == typeof(bool))
            {
                minValue = (T)(object)false;
                maxValue = (T)(object)true;
            }
            else
            {
                minValue = type.GetStaticFieldValue<T>(nameof(int.MinValue));
                maxValue = type.GetStaticFieldValue<T>(nameof(int.MaxValue));
            }
        }
[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetSignedAndUnsignedVersions(Type type, bool isSigned, out Type signedVersion, out Type unsignedVersion)
        {
            if (isSigned)
            {
                signedVersion = type;
                unsignedVersion = type.GetUnsignedVersionOrNull();
            }
            else
            {
                signedVersion = type.GetSignedVersionOrNull();
                unsignedVersion = type;
            }
        }
    }
}
