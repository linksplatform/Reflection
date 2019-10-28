#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Platform.Reflection
{
    public static class TypeBuilderExtensions
    {
        public static readonly MethodAttributes DefaultStaticMethodAttributes = MethodAttributes.Public | MethodAttributes.Static;
        public static readonly MethodAttributes DefaultVirtualMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig;
        public static readonly MethodImplAttributes DefaultMethodImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitMethod<TDelegate>(this TypeBuilder type, string methodName, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, Action<ILGenerator> emitCode)
        {
            typeof(TDelegate).GetDelegateCharacteristics(out Type returnType, out Type[] parameterTypes);
            EmitMethod(type, methodName, methodAttributes, methodImplAttributes, returnType, parameterTypes, emitCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitMethod(this TypeBuilder type, string methodName, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, Type returnType, Type[] parameterTypes, Action<ILGenerator> emitCode)
        {
            MethodBuilder method = type.DefineMethod(methodName, methodAttributes, returnType, parameterTypes);
            method.SetImplementationFlags(methodImplAttributes);
            var generator = method.GetILGenerator();
            emitCode(generator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitStaticMethod<TDelegate>(this TypeBuilder type, string methodName, Action<ILGenerator> emitCode) => type.EmitMethod<TDelegate>(methodName, DefaultStaticMethodAttributes, DefaultMethodImplAttributes, emitCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitVirtualMethod<TDelegate>(this TypeBuilder type, string methodName, Action<ILGenerator> emitCode) => type.EmitMethod<TDelegate>(methodName, DefaultVirtualMethodAttributes, DefaultMethodImplAttributes, emitCode);
    }
}
