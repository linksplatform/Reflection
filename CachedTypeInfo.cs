﻿using System;
using System.Runtime.InteropServices;

// ReSharper disable AssignmentInConditionalExpression

// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable StaticFieldInGenericType

namespace Platform.Reflection
{
    public class CachedTypeInfo<T>
    {
        public static readonly bool IsSupported;
        public static readonly Type Type;
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

        static CachedTypeInfo()
        {
            Type = typeof(T);

            if (IsNullable = Type.IsNullable()) //-V3055
                UnderlyingType = Nullable.GetUnderlyingType(Type);
            else
                UnderlyingType = Type;

            try
            {
                bool canBeNumeric = false, isNumeric = false, isSigned = false, isFloatPoint = false;

                if (UnderlyingType == typeof(decimal) ||
                    UnderlyingType == typeof(double) ||
                    UnderlyingType == typeof(float))
                    canBeNumeric = isNumeric = isSigned = isFloatPoint = true;
                else if (UnderlyingType == typeof(sbyte) ||
                    UnderlyingType == typeof(short) ||
                    UnderlyingType == typeof(int) ||
                    UnderlyingType == typeof(long))
                    canBeNumeric = isNumeric = isSigned = true;
                else if (UnderlyingType == typeof(byte) ||
                    UnderlyingType == typeof(ushort) ||
                    UnderlyingType == typeof(uint) ||
                    UnderlyingType == typeof(ulong))
                    canBeNumeric = isNumeric = true;
                else if (UnderlyingType == typeof(bool) ||
                    UnderlyingType == typeof(char) ||
                    UnderlyingType == typeof(DateTime) ||
                    UnderlyingType == typeof(TimeSpan))
                    canBeNumeric = true;

                int bitsLength = Marshal.SizeOf(UnderlyingType) * 8;

                T minValue, maxValue;

                if (UnderlyingType == typeof(bool))
                {
                    minValue = (T)(object)false;
                    maxValue = (T)(object)true;
                }
                else
                {
                    minValue = UnderlyingType.GetStaticFieldValue<T>("MinValue");
                    maxValue = UnderlyingType.GetStaticFieldValue<T>("MaxValue");
                }

                Type signedVersion, unsignedVersion;

                if (isSigned)
                {
                    signedVersion = UnderlyingType;
                    unsignedVersion = UnderlyingType.GetUnsignedVersionOrNull();
                }
                else
                {
                    signedVersion = UnderlyingType.GetSignedVersionOrNull();
                    unsignedVersion = UnderlyingType;
                }

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
            catch (Exception)
            {
            }
        }
    }
}
