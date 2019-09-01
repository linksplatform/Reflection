using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public static class ILGeneratorExtensions
    {
        public static void Throw<T>(this ILGenerator generator) => generator.ThrowException(typeof(T));

        public static void ConvertTo<T>(this ILGenerator generator)
        {
            var type = typeof(T);
            if (type == typeof(short))
            {
                generator.Emit(OpCodes.Conv_I2);
            }
            else if (type == typeof(ushort))
            {
                generator.Emit(OpCodes.Conv_U2);
            }
            else if (type == typeof(sbyte))
            {
                generator.Emit(OpCodes.Conv_I1);
            }
            else if (type == typeof(byte))
            {
                generator.Emit(OpCodes.Conv_U1);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static void LoadConstant(this ILGenerator generator, bool value) => generator.LoadConstant(value ? 1 : 0);

        public static void LoadConstant(this ILGenerator generator, float value) => generator.Emit(OpCodes.Ldc_R4, value);

        public static void LoadConstant(this ILGenerator generator, double value) => generator.Emit(OpCodes.Ldc_R8, value);

        public static void LoadConstant(this ILGenerator generator, ulong value) => generator.Emit(OpCodes.Ldc_I8, unchecked((long)value));

        public static void LoadConstant(this ILGenerator generator, long value) => generator.Emit(OpCodes.Ldc_I8, value);

        public static void LoadConstant(this ILGenerator generator, uint value)
        {
            switch (value)
            {
                case uint.MaxValue:
                    generator.Emit(OpCodes.Ldc_I4_M1, value);
                    return;
                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0, value);
                    return;
                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1, value);
                    return;
                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2, value);
                    return;
                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3, value);
                    return;
                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4, value);
                    return;
                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5, value);
                    return;
                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6, value);
                    return;
                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7, value);
                    return;
                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8, value);
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

        public static void LoadConstant(this ILGenerator generator, int value)
        {
            switch (value)
            {
                case -1:
                    generator.Emit(OpCodes.Ldc_I4_M1, value);
                    return;
                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0, value);
                    return;
                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1, value);
                    return;
                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2, value);
                    return;
                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3, value);
                    return;
                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4, value);
                    return;
                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5, value);
                    return;
                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6, value);
                    return;
                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7, value);
                    return;
                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8, value);
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

        public static void LoadConstant(this ILGenerator generator, short value)
        {
            generator.LoadConstant((int)value);
            generator.ConvertTo<short>();
        }

        public static void LoadConstant(this ILGenerator generator, ushort value)
        {
            generator.LoadConstant((int)value);
            generator.ConvertTo<ushort>();
        }

        public static void LoadConstant(this ILGenerator generator, sbyte value)
        {
            generator.LoadConstant((int)value);
            generator.ConvertTo<sbyte>();
        }

        public static void LoadConstant(this ILGenerator generator, byte value)
        {
            generator.LoadConstant((int)value);
            generator.ConvertTo<byte>();
        }

        public static void LoadConstantOne<TConstant>(this ILGenerator generator) => LoadConstantOne(generator, typeof(TConstant));

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

        public static void LoadConstant<TConstant>(this ILGenerator generator, object constantValue) => LoadConstant(generator, typeof(TConstant), constantValue);

        public static void LoadConstant(this ILGenerator generator, Type constantType, object constantValue)
        {
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

        public static void Increment<TValue>(this ILGenerator generator) => generator.Increment(typeof(TValue));

        public static void Decrement<TValue>(this ILGenerator generator) => generator.Decrement(typeof(TValue));

        public static void Increment(this ILGenerator generator, Type valueType)
        {
            generator.LoadConstantOne(valueType);
            generator.Add();
        }

        public static void Add(this ILGenerator generator) => generator.Emit(OpCodes.Add);

        public static void Decrement(this ILGenerator generator, Type valueType)
        {
            generator.LoadConstantOne(valueType);
            generator.Subtract();
        }

        public static void Subtract(this ILGenerator generator) => generator.Emit(OpCodes.Sub);

        public static void Negate(this ILGenerator generator) => generator.Emit(OpCodes.Neg);

        public static void And(this ILGenerator generator) => generator.Emit(OpCodes.And);

        public static void Or(this ILGenerator generator) => generator.Emit(OpCodes.Or);

        public static void Not(this ILGenerator generator) => generator.Emit(OpCodes.Not);

        public static void ShiftLeft(this ILGenerator generator) => generator.Emit(OpCodes.Shl);

        public static void ShiftRight(this ILGenerator generator) => generator.Emit(OpCodes.Shr);

        public static void LoadArgument(this ILGenerator generator, int argumentIndex)
        {
            if (argumentIndex == 0)
            {
                generator.Emit(OpCodes.Ldarg_0);
            }
            else if (argumentIndex == 1)
            {
                generator.Emit(OpCodes.Ldarg_1);
            }
            else if (argumentIndex == 2)
            {
                generator.Emit(OpCodes.Ldarg_2);
            }
            else if (argumentIndex == 3)
            {
                generator.Emit(OpCodes.Ldarg_3);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg, argumentIndex);
            }
        }

        public static void LoadArguments(this ILGenerator generator, params int[] argumentIndices)
        {
            for (var i = 0; i < argumentIndices.Length; i++)
            {
                generator.LoadArgument(argumentIndices[i]);
            }
        }

        public static void StoreArgument(this ILGenerator generator, int argumentIndex) => generator.Emit(OpCodes.Starg, argumentIndex);

        public static void CompareGreaterThan(this ILGenerator generator) => generator.Emit(OpCodes.Cgt);

        public static void UnsignedCompareGreaterThan(this ILGenerator generator) => generator.Emit(OpCodes.Cgt_Un);

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

        public static void CompareLessThan(this ILGenerator generator) => generator.Emit(OpCodes.Clt);

        public static void UnsignedCompareLessThan(this ILGenerator generator) => generator.Emit(OpCodes.Clt_Un);

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

        public static void BranchIfGreaterOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Bge, label);

        public static void UnsignedBranchIfGreaterOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Bge_Un, label);

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

        public static void BranchIfLessOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Ble, label);

        public static void UnsignedBranchIfLessOrEqual(this ILGenerator generator, Label label) => generator.Emit(OpCodes.Ble_Un, label);

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

        public static void Box<TBox>(this ILGenerator generator) => generator.Box(typeof(TBox));

        public static void Box(this ILGenerator generator, Type boxedType) => generator.Emit(OpCodes.Box, boxedType);

        public static void Call(this ILGenerator generator, MethodInfo method) => generator.Emit(OpCodes.Call, method);

        public static void Return(this ILGenerator generator) => generator.Emit(OpCodes.Ret);

        public static void Unbox<TUnbox>(this ILGenerator generator) => generator.Unbox(typeof(TUnbox));

        public static void Unbox(this ILGenerator generator, Type typeToUnbox) => generator.Emit(OpCodes.Unbox, typeToUnbox);

        public static void UnboxValue<TUnbox>(this ILGenerator generator) => generator.UnboxValue(typeof(TUnbox));

        public static void UnboxValue(this ILGenerator generator, Type typeToUnbox) => generator.Emit(OpCodes.Unbox_Any, typeToUnbox);

        public static LocalBuilder DeclareLocal<T>(this ILGenerator generator) => generator.DeclareLocal(typeof(T));

        public static void LoadLocal(this ILGenerator generator, LocalBuilder local) => generator.Emit(OpCodes.Ldloc, local);

        public static void StoreLocal(this ILGenerator generator, LocalBuilder local) => generator.Emit(OpCodes.Stloc, local);

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

        public static void NewObject(this ILGenerator generator, ConstructorInfo constructor)
        {
            generator.Emit(OpCodes.Newobj, constructor);
        }

        public static void LoadIndirect<T>(this ILGenerator generator, bool isVolatile = false, byte? unaligned = null) => generator.LoadIndirect(typeof(T), isVolatile, unaligned);

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

        public static void StoreIndirect<T>(this ILGenerator generator, bool isVolatile = false, byte? unaligned = null) => generator.StoreIndirect(typeof(T), isVolatile, unaligned);

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
    }
}
