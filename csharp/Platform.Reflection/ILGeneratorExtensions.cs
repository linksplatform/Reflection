using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the il generator extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class ILGeneratorExtensions
    {
        /// <summary>
        /// <para>
        /// Throws the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Throw<T>(this ILGenerator generator) => generator.ThrowException(typeof(T));

        /// <summary>
        /// <para>
        /// Uncheckeds the convert using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TSource">
        /// <para>The source.</para>
        /// <para></para>
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// <para>The target.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UncheckedConvert<TSource, TTarget>(this ILGenerator generator) => UncheckedConvert<TSource, TTarget>(generator, extendSign: false);

        /// <summary>
        /// <para>
        /// Uncheckeds the convert using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TSource">
        /// <para>The source.</para>
        /// <para></para>
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// <para>The target.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="extendSign">
        /// <para>The extend sign.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UncheckedConvert<TSource, TTarget>(this ILGenerator generator, bool extendSign)
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);
            if (sourceType == targetType)
            {
                return;
            }
            if (extendSign)
            {
                if (sourceType == typeof(byte))
                {
                    generator.Emit(OpCodes.Conv_I1);
                }
                if (sourceType == typeof(ushort) || sourceType == typeof(char))
                {
                    generator.Emit(OpCodes.Conv_I2);
                }
            }
            if (NumericType<TSource>.BitsSize > NumericType<TTarget>.BitsSize)
            {
                generator.ConvertToInteger<TSource>(targetType, extendSign: false);
            }
            else
            {
                generator.ConvertToInteger<TSource>(targetType, extendSign);
            }
            if (targetType == typeof(float))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_R4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_R_Un);
                }
            }
            else if (targetType == typeof(double))
            {
                generator.Emit(OpCodes.Conv_R8);
            }
            else if (targetType == typeof(bool))
            {
                generator.ConvertToBoolean<TSource>();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ConvertToBoolean<TSource>(this ILGenerator generator)
        {
            generator.LoadConstant<TSource>(default);
            var sourceType = typeof(TSource);
            if (sourceType == typeof(float) || sourceType == typeof(double))
            {
                generator.Emit(OpCodes.Ceq);
                // Inversion of the first Ceq instruction
                generator.LoadConstant<int>(0);
                generator.Emit(OpCodes.Ceq);
            }
            else
            {
                generator.Emit(OpCodes.Cgt_Un);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ConvertToInteger<TSource>(this ILGenerator generator, Type targetType, bool extendSign)
        {
            if (targetType == typeof(sbyte))
            {
                generator.Emit(OpCodes.Conv_I1);
            }
            else if (targetType == typeof(byte))
            {
                generator.Emit(OpCodes.Conv_U1);
            }
            else if (targetType == typeof(short))
            {
                generator.Emit(OpCodes.Conv_I2);
            }
            else if (targetType == typeof(ushort) || targetType == typeof(char))
            {
                var sourceType = typeof(TSource);
                if (sourceType != typeof(ushort) && sourceType != typeof(char))
                {
                    generator.Emit(OpCodes.Conv_U2);
                }
            }
            else if (targetType == typeof(int))
            {
                generator.Emit(OpCodes.Conv_I4);
            }
            else if (targetType == typeof(uint))
            {
                generator.Emit(OpCodes.Conv_U4);
            }
            else if (targetType == typeof(long) || targetType == typeof(ulong))
            {
                if (NumericType<TSource>.IsSigned || extendSign)
                {
                    generator.Emit(OpCodes.Conv_I8);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_U8);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Checkeds the convert using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TSource">
        /// <para>The source.</para>
        /// <para></para>
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// <para>The target.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckedConvert<TSource, TTarget>(this ILGenerator generator)
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);
            if (sourceType == targetType)
            {
                return;
            }
            if (targetType == typeof(short))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I2);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I2_Un);
                }
            }
            else if (targetType == typeof(ushort) || targetType == typeof(char))
            {
                if (sourceType != typeof(ushort) && sourceType != typeof(char))
                {
                    if (NumericType<TSource>.IsSigned)
                    {
                        generator.Emit(OpCodes.Conv_Ovf_U2);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Conv_Ovf_U2_Un);
                    }
                }
            }
            else if (targetType == typeof(sbyte))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I1);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I1_Un);
                }
            }
            else if (targetType == typeof(byte))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U1);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U1_Un);
                }
            }
            else if (targetType == typeof(int))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I4_Un);
                }
            }
            else if (targetType == typeof(uint))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U4_Un);
                }
            }
            else if (targetType == typeof(long))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_I8);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_I8_Un);
                }
            }
            else if (targetType == typeof(ulong))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_Ovf_U8);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_Ovf_U8_Un);
                }
            }
            else if (targetType == typeof(float))
            {
                if (NumericType<TSource>.IsSigned)
                {
                    generator.Emit(OpCodes.Conv_R4);
                }
                else
                {
                    generator.Emit(OpCodes.Conv_R_Un);
                }
            }
            else if (targetType == typeof(double))
            {
                generator.Emit(OpCodes.Conv_R8);
            }
            else if (targetType == typeof(bool))
            {
                generator.ConvertToBoolean<TSource>();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, bool value) => generator.LoadConstant(value ? 1 : 0);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, float value) => generator.Emit(OpCodes.Ldc_R4, value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, double value) => generator.Emit(OpCodes.Ldc_R8, value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, ulong value) => generator.Emit(OpCodes.Ldc_I8, unchecked((long)value));

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, long value) => generator.Emit(OpCodes.Ldc_I8, value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, uint value)
        {
            switch (value)
            {
                case uint.MaxValue:
                    generator.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8);
                    return;
                default:
                    if (value <= sbyte.MaxValue)
                    {
                        generator.Emit(OpCodes.Ldc_I4_S, unchecked((byte)value));
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldc_I4, unchecked((int)value));
                    }
                    return;
            }
        }

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, int value)
        {
            switch (value)
            {
                case -1:
                    generator.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8);
                    return;
                default:
                    if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
                    {
                        generator.Emit(OpCodes.Ldc_I4_S, unchecked((byte)value));
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldc_I4, value);
                    }
                    return;
            }
        }

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, short value) => generator.LoadConstant((int)value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, ushort value) => generator.LoadConstant((int)value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, sbyte value) => generator.LoadConstant((int)value);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="value">
        /// <para>The value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, byte value) => generator.LoadConstant((int)value);

        /// <summary>
        /// <para>
        /// Loads the constant one using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TConstant">
        /// <para>The constant.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstantOne<TConstant>(this ILGenerator generator) => LoadConstantOne(generator, typeof(TConstant));

        /// <summary>
        /// <para>
        /// Loads the constant one using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="constantType">
        /// <para>The constant type.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstantOne(this ILGenerator generator, Type constantType)
        {
            if (constantType == typeof(float))
            {
                generator.LoadConstant(1F);
            }
            else if (constantType == typeof(double))
            {
                generator.LoadConstant(1D);
            }
            else if (constantType == typeof(long))
            {
                generator.LoadConstant(1L);
            }
            else if (constantType == typeof(ulong))
            {
                generator.LoadConstant(1UL);
            }
            else if (constantType == typeof(int))
            {
                generator.LoadConstant(1);
            }
            else if (constantType == typeof(uint))
            {
                generator.LoadConstant(1U);
            }
            else if (constantType == typeof(short))
            {
                generator.LoadConstant((short)1);
            }
            else if (constantType == typeof(ushort))
            {
                generator.LoadConstant((ushort)1);
            }
            else if (constantType == typeof(sbyte))
            {
                generator.LoadConstant((sbyte)1);
            }
            else if (constantType == typeof(byte))
            {
                generator.LoadConstant((byte)1);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TConstant">
        /// <para>The constant.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="constantValue">
        /// <para>The constant value.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant<TConstant>(this ILGenerator generator, TConstant constantValue) => LoadConstant(generator, typeof(TConstant), constantValue);

        /// <summary>
        /// <para>
        /// Loads the constant using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="constantType">
        /// <para>The constant type.</para>
        /// <para></para>
        /// </param>
        /// <param name="constantValue">
        /// <para>The constant value.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadConstant(this ILGenerator generator, Type constantType, object constantValue)
        {
            constantValue = Convert.ChangeType(constantValue, constantType);
            if (constantType == typeof(float))
            {
                generator.LoadConstant((float)constantValue);
            }
            else if (constantType == typeof(double))
            {
                generator.LoadConstant((double)constantValue);
            }
            else if (constantType == typeof(long))
            {
                generator.LoadConstant((long)constantValue);
            }
            else if (constantType == typeof(ulong))
            {
                generator.LoadConstant((ulong)constantValue);
            }
            else if (constantType == typeof(int))
            {
                generator.LoadConstant((int)constantValue);
            }
            else if (constantType == typeof(uint))
            {
                generator.LoadConstant((uint)constantValue);
            }
            else if (constantType == typeof(short))
            {
                generator.LoadConstant((short)constantValue);
            }
            else if (constantType == typeof(ushort))
            {
                generator.LoadConstant((ushort)constantValue);
            }
            else if (constantType == typeof(sbyte))
            {
                generator.LoadConstant((sbyte)constantValue);
            }
            else if (constantType == typeof(byte))
            {
                generator.LoadConstant((byte)constantValue);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// <para>
        /// Increments the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TValue">
        /// <para>The value.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Increment<TValue>(this ILGenerator generator) => generator.Increment(typeof(TValue));

        /// <summary>
        /// <para>
        /// Decrements the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TValue">
        /// <para>The value.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decrement<TValue>(this ILGenerator generator) => generator.Decrement(typeof(TValue));

        /// <summary>
        /// <para>
        /// Increments the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="valueType">
        /// <para>The value type.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Increment(this ILGenerator generator, Type valueType)
        {
            generator.LoadConstantOne(valueType);
            generator.Add();
        }

        /// <summary>
        /// <para>
        /// Adds the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ILGenerator generator) => generator.Emit(OpCodes.Add);

        /// <summary>
        /// <para>
        /// Decrements the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="valueType">
        /// <para>The value type.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decrement(this ILGenerator generator, Type valueType)
        {
            generator.LoadConstantOne(valueType);
            generator.Subtract();
        }

        /// <summary>
        /// <para>
        /// Subtracts the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(this ILGenerator generator) => generator.Emit(OpCodes.Sub);

        /// <summary>
        /// <para>
        /// Negates the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(this ILGenerator generator) => generator.Emit(OpCodes.Neg);

        /// <summary>
        /// <para>
        /// Ands the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And(this ILGenerator generator) => generator.Emit(OpCodes.And);

        /// <summary>
        /// <para>
        /// Ors the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or(this ILGenerator generator) => generator.Emit(OpCodes.Or);

        /// <summary>
        /// <para>
        /// Nots the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Not(this ILGenerator generator) => generator.Emit(OpCodes.Not);

        /// <summary>
        /// <para>
        /// Shifts the left using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShiftLeft(this ILGenerator generator) => generator.Emit(OpCodes.Shl);

        /// <summary>
        /// <para>
        /// Shifts the right using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShiftRight<T>(this ILGenerator generator)
        {
            generator.Emit(NumericType<T>.IsSigned ? OpCodes.Shr : OpCodes.Shr_Un);
        }

        /// <summary>
        /// <para>
        /// Loads the argument using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="argumentIndex">
        /// <para>The argument index.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadArgument(this ILGenerator generator, int argumentIndex)
        {
            switch (argumentIndex)
            {
                case 0:
                    generator.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    generator.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    generator.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    generator.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    generator.Emit(OpCodes.Ldarg, argumentIndex);
                    break;
            }
        }

        /// <summary>
        /// <para>
        /// Loads the arguments using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="argumentIndices">
        /// <para>The argument indices.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadArguments(this ILGenerator generator, params int[] argumentIndices)
        {
            for (var i = 0; i < argumentIndices.Length; i++)
            {
                generator.LoadArgument(argumentIndices[i]);
            }
        }

        /// <summary>
        /// <para>
        /// Stores the argument using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="argumentIndex">
        /// <para>The argument index.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreArgument(this ILGenerator generator, int argumentIndex) => generator.Emit(OpCodes.Starg, argumentIndex);

        /// <summary>
        /// <para>
        /// Compares the greater than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompareGreaterThan(this ILGenerator generator) => generator.Emit(OpCodes.Cgt);

        /// <summary>
        /// <para>
        /// Unsigneds the compare greater than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsignedCompareGreaterThan(this ILGenerator generator) => generator.Emit(OpCodes.Cgt_Un);

        /// <summary>
        /// <para>
        /// Compares the greater than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isSigned">
        /// <para>The is signed.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompareGreaterThan(this ILGenerator generator, bool isSigned)
        {
            if (isSigned)
            {
                generator.CompareGreaterThan();
            }
            else
            {
                generator.UnsignedCompareGreaterThan();
            }
        }

        /// <summary>
        /// <para>
        /// Compares the less than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompareLessThan(this ILGenerator generator) => generator.Emit(OpCodes.Clt);

        /// <summary>
        /// <para>
        /// Unsigneds the compare less than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsignedCompareLessThan(this ILGenerator generator) => generator.Emit(OpCodes.Clt_Un);

        /// <summary>
        /// <para>
        /// Compares the less than using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isSigned">
        /// <para>The is signed.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CompareLessThan(this ILGenerator generator, bool isSigned)
        {
            if (isSigned)
            {
                generator.CompareLessThan();
            }
            else
            {
                generator.UnsignedCompareLessThan();
            }
        }

        /// <summary>
        /// <para>
        /// Branches the if greater or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BranchIfGreaterOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Bge, label);

        /// <summary>
        /// <para>
        /// Unsigneds the branch if greater or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsignedBranchIfGreaterOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Bge_Un, label);

        /// <summary>
        /// <para>
        /// Branches the if greater or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isSigned">
        /// <para>The is signed.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BranchIfGreaterOrEqual(this ILGenerator generator, bool isSigned, Label label)
        {
            if (isSigned)
            {
                generator.BranchIfGreaterOrEqual(label);
            }
            else
            {
                generator.UnsignedBranchIfGreaterOrEqual(label);
            }
        }

        /// <summary>
        /// <para>
        /// Branches the if less or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BranchIfLessOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Ble, label);

        /// <summary>
        /// <para>
        /// Unsigneds the branch if less or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsignedBranchIfLessOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Ble_Un, label);

        /// <summary>
        /// <para>
        /// Branches the if less or equal using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isSigned">
        /// <para>The is signed.</para>
        /// <para></para>
        /// </param>
        /// <param name="label">
        /// <para>The label.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BranchIfLessOrEqual(this ILGenerator generator, bool isSigned, Label label)
        {
            if (isSigned)
            {
                generator.BranchIfLessOrEqual(label);
            }
            else
            {
                generator.UnsignedBranchIfLessOrEqual(label);
            }
        }

        /// <summary>
        /// <para>
        /// Boxes the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TBox">
        /// <para>The box.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Box<TBox>(this ILGenerator generator) => generator.Box(typeof(TBox));

        /// <summary>
        /// <para>
        /// Boxes the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="boxedType">
        /// <para>The boxed type.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Box(this ILGenerator generator, Type boxedType) => generator.Emit(OpCodes.Box, boxedType);

        /// <summary>
        /// <para>
        /// Calls the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="method">
        /// <para>The method.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Call(this ILGenerator generator, MethodInfo method) => generator.Emit(OpCodes.Call, method);

        /// <summary>
        /// <para>
        /// Returns the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(this ILGenerator generator) => generator.Emit(OpCodes.Ret);

        /// <summary>
        /// <para>
        /// Unboxes the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TUnbox">
        /// <para>The unbox.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unbox<TUnbox>(this ILGenerator generator) => generator.Unbox(typeof(TUnbox));

        /// <summary>
        /// <para>
        /// Unboxes the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="typeToUnbox">
        /// <para>The type to unbox.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unbox(this ILGenerator generator, Type typeToUnbox) => generator.Emit(OpCodes.Unbox, typeToUnbox);

        /// <summary>
        /// <para>
        /// Unboxes the value using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TUnbox">
        /// <para>The unbox.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnboxValue<TUnbox>(this ILGenerator generator) => generator.UnboxValue(typeof(TUnbox));

        /// <summary>
        /// <para>
        /// Unboxes the value using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="typeToUnbox">
        /// <para>The type to unbox.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnboxValue(this ILGenerator generator, Type typeToUnbox) => generator.Emit(OpCodes.Unbox_Any, typeToUnbox);

        /// <summary>
        /// <para>
        /// Declares the local using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The local builder</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LocalBuilder DeclareLocal<T>(this ILGenerator generator) => generator.DeclareLocal(typeof(T));

        /// <summary>
        /// <para>
        /// Loads the local using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="local">
        /// <para>The local.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadLocal(this ILGenerator generator, LocalBuilder local) => generator.Emit(OpCodes.Ldloc, local);

        /// <summary>
        /// <para>
        /// Stores the local using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="local">
        /// <para>The local.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreLocal(this ILGenerator generator, LocalBuilder local) => generator.Emit(OpCodes.Stloc, local);

        /// <summary>
        /// <para>
        /// News the object using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="parameterTypes">
        /// <para>The parameter types.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NewObject(this ILGenerator generator, Type type, params Type[] parameterTypes)
        {
            var allConstructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
#if !NETSTANDARD
                | BindingFlags.CreateInstance
#endif
                );
            var constructor = allConstructors.Where(c => c.GetParameters().Length == parameterTypes.Length && c.GetParameters().Select((p, i) => p.ParameterType == parameterTypes[i]).Aggregate(true, (a, b) => a && b)).SingleOrDefault();
            if (constructor == null)
            {
                throw new InvalidOperationException("Type " + type + " must have a constructor that matches parameters [" + string.Join(", ", parameterTypes.AsEnumerable()) + "]");
            }
            generator.NewObject(constructor);
        }

        /// <summary>
        /// <para>
        /// News the object using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="constructor">
        /// <para>The constructor.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NewObject(this ILGenerator generator, ConstructorInfo constructor) => generator.Emit(OpCodes.Newobj, constructor);

        /// <summary>
        /// <para>
        /// Loads the indirect using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isVolatile">
        /// <para>The is volatile.</para>
        /// <para></para>
        /// </param>
        /// <param name="unaligned">
        /// <para>The unaligned.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadIndirect<T>(this ILGenerator generator, bool isVolatile = false, byte? unaligned = null) => generator.LoadIndirect(typeof(T), isVolatile, unaligned);

        /// <summary>
        /// <para>
        /// Loads the indirect using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="isVolatile">
        /// <para>The is volatile.</para>
        /// <para></para>
        /// </param>
        /// <param name="unaligned">
        /// <para>The unaligned.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para>unaligned must be null, 1, 2, or 4</para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadIndirect(this ILGenerator generator, Type type, bool isVolatile = false, byte? unaligned = null)
        {
            if (unaligned.HasValue && unaligned != 1 && unaligned != 2 && unaligned != 4)
            {
                throw new ArgumentException("unaligned must be null, 1, 2, or 4");
            }
            if (isVolatile)
            {
                generator.Emit(OpCodes.Volatile);
            }
            if (unaligned.HasValue)
            {
                generator.Emit(OpCodes.Unaligned, unaligned.Value);
            }
            if (type.IsPointer)
            {
                generator.Emit(OpCodes.Ldind_I);
            }
            else if (!type.IsValueType)
            {
                generator.Emit(OpCodes.Ldind_Ref);
            }
            else if (type == typeof(sbyte))
            {
                generator.Emit(OpCodes.Ldind_I1);
            }
            else if (type == typeof(bool))
            {
                generator.Emit(OpCodes.Ldind_I1);
            }
            else if (type == typeof(byte))
            {
                generator.Emit(OpCodes.Ldind_U1);
            }
            else if (type == typeof(short))
            {
                generator.Emit(OpCodes.Ldind_I2);
            }
            else if (type == typeof(ushort))
            {
                generator.Emit(OpCodes.Ldind_U2);
            }
            else if (type == typeof(char))
            {
                generator.Emit(OpCodes.Ldind_U2);
            }
            else if (type == typeof(int))
            {
                generator.Emit(OpCodes.Ldind_I4);
            }
            else if (type == typeof(uint))
            {
                generator.Emit(OpCodes.Ldind_U4);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                generator.Emit(OpCodes.Ldind_I8);
            }
            else if (type == typeof(float))
            {
                generator.Emit(OpCodes.Ldind_R4);
            }
            else if (type == typeof(double))
            {
                generator.Emit(OpCodes.Ldind_R8);
            }
            else
            {
                throw new InvalidOperationException("LoadIndirect cannot be used with " + type + ", LoadObject may be more appropriate");
            }
        }

        /// <summary>
        /// <para>
        /// Stores the indirect using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="isVolatile">
        /// <para>The is volatile.</para>
        /// <para></para>
        /// </param>
        /// <param name="unaligned">
        /// <para>The unaligned.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreIndirect<T>(this ILGenerator generator, bool isVolatile = false, byte? unaligned = null) => generator.StoreIndirect(typeof(T), isVolatile, unaligned);

        /// <summary>
        /// <para>
        /// Stores the indirect using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="isVolatile">
        /// <para>The is volatile.</para>
        /// <para></para>
        /// </param>
        /// <param name="unaligned">
        /// <para>The unaligned.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para>unaligned must be null, 1, 2, or 4</para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreIndirect(this ILGenerator generator, Type type, bool isVolatile = false, byte? unaligned = null)
        {
            if (unaligned.HasValue && unaligned != 1 && unaligned != 2 && unaligned != 4)
            {
                throw new ArgumentException("unaligned must be null, 1, 2, or 4");
            }
            if (isVolatile)
            {
                generator.Emit(OpCodes.Volatile);
            }
            if (unaligned.HasValue)
            {
                generator.Emit(OpCodes.Unaligned, unaligned.Value);
            }
            if (type.IsPointer)
            {
                generator.Emit(OpCodes.Stind_I);
            }
            else if (!type.IsValueType)
            {
                generator.Emit(OpCodes.Stind_Ref);
            }
            else if (type == typeof(sbyte) || type == typeof(byte))
            {
                generator.Emit(OpCodes.Stind_I1);
            }
            else if (type == typeof(short) || type == typeof(ushort))
            {
                generator.Emit(OpCodes.Stind_I2);
            }
            else if (type == typeof(int) || type == typeof(uint))
            {
                generator.Emit(OpCodes.Stind_I4);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                generator.Emit(OpCodes.Stind_I8);
            }
            else if (type == typeof(float))
            {
                generator.Emit(OpCodes.Stind_R4);
            }
            else if (type == typeof(double))
            {
                generator.Emit(OpCodes.Stind_R8);
            }
            else
            {
                throw new InvalidOperationException("StoreIndirect cannot be used with " + type + ", StoreObject may be more appropriate");
            }
        }

        /// <summary>
        /// <para>
        /// Multiplies the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Mul);
        }

        /// <summary>
        /// <para>
        /// Checkeds the multiply using the specified generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckedMultiply<T>(this ILGenerator generator)
        {
            if (NumericType<T>.IsSigned)
            {
                generator.Emit(OpCodes.Mul_Ovf);
            }
            else
            {
                generator.Emit(OpCodes.Mul_Ovf_Un);
            }
        }

        /// <summary>
        /// <para>
        /// Divides the generator.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="generator">
        /// <para>The generator.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(this ILGenerator generator)
        {
            if (NumericType<T>.IsSigned)
            {
                generator.Emit(OpCodes.Div);
            }
            else
            {
                generator.Emit(OpCodes.Div_Un);
            }
        }
    }
}
