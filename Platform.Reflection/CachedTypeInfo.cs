using System;
using System.Runtime.InteropServices;
using Platform.Exceptions;

// ReSharper disable AssignmentInConditionalExpression
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable StaticFieldInGenericType
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public class Type<T>
    {
        public static readonly bool IsSupported;
        public static readonly Type TypeObject;
        public static readonly Type UnderlyingType;
        public static readonly Type SignedVersion;
        public static readonly Type UnsignedVersion;
        public static readonly bool IsFloatPoint;
        public static readonly bool IsNumeric;
        public static readonly bool IsSigned;
        public static readonly bool CanBeNumeric;
        public static readonly bool IsNullable;
        public static readonly int BitsLength;
        public static readonly T MinValue;
        public static readonly T MaxValue;

        static Type()
        {
            try
            {
                TypeObject = typeof(T);
                IsNullable = TypeObject.IsNullable();
                UnderlyingType = IsNullable ? Nullable.GetUnderlyingType(TypeObject) : TypeObject;
                var canBeNumeric = UnderlyingType.CanBeNumeric();
                var isNumeric = UnderlyingType.IsNumeric();
                var isSigned = UnderlyingType.IsSigned();
                var isFloatPoint = UnderlyingType.IsFloatPoint();
                var bitsLength = Marshal.SizeOf(UnderlyingType) * 8;
                GetMinAndMaxValues(UnderlyingType, out T minValue, out T maxValue);
                GetSignedAndUnsignedVersions(UnderlyingType, isSigned, out Type signedVersion, out Type unsignedVersion);
                IsSupported = true;
                CanBeNumeric = canBeNumeric;
                IsNumeric = isNumeric;
                IsSigned = isSigned;
                IsFloatPoint = isFloatPoint;
                BitsLength = bitsLength;
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

        private static void GetMinAndMaxValues(Type type, out T minValue, out T maxValue)
        {
            if (type == typeof(bool))
            {
                minValue = (T)(object)false;
                maxValue = (T)(object)true;
            }
            else
            {
                minValue = type.GetStaticFieldValue<T>("MinValue");
                maxValue = type.GetStaticFieldValue<T>("MaxValue");
            }
        }

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
