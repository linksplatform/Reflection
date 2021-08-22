#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the type builder extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class TypeBuilderExtensions
    {
        /// <summary>
        /// <para>
        /// The static.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly MethodAttributes DefaultStaticMethodAttributes = MethodAttributes.Public | MethodAttributes.Static;
        /// <summary>
        /// <para>
        /// The hide by sig.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly MethodAttributes DefaultFinalVirtualMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig;
        /// <summary>
        /// <para>
        /// The aggressive inlining.
        /// </para>
        /// <para></para>
        /// </summary>
        public static readonly MethodImplAttributes DefaultMethodImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining;

        /// <summary>
        /// <para>
        /// Emits the method using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TDelegate">
        /// <para>The delegate.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodName">
        /// <para>The method name.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodAttributes">
        /// <para>The method attributes.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodImplAttributes">
        /// <para>The method impl attributes.</para>
        /// <para></para>
        /// </param>
        /// <param name="emitCode">
        /// <para>The emit code.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitMethod<TDelegate>(this TypeBuilder type, string methodName, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, Action<ILGenerator> emitCode)
        {
            typeof(TDelegate).GetDelegateCharacteristics(out Type returnType, out Type[] parameterTypes);
            EmitMethod(type, methodName, methodAttributes, methodImplAttributes, returnType, parameterTypes, emitCode);
        }

        /// <summary>
        /// <para>
        /// Emits the method using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodName">
        /// <para>The method name.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodAttributes">
        /// <para>The method attributes.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodImplAttributes">
        /// <para>The method impl attributes.</para>
        /// <para></para>
        /// </param>
        /// <param name="returnType">
        /// <para>The return type.</para>
        /// <para></para>
        /// </param>
        /// <param name="parameterTypes">
        /// <para>The parameter types.</para>
        /// <para></para>
        /// </param>
        /// <param name="emitCode">
        /// <para>The emit code.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitMethod(this TypeBuilder type, string methodName, MethodAttributes methodAttributes, MethodImplAttributes methodImplAttributes, Type returnType, Type[] parameterTypes, Action<ILGenerator> emitCode)
        {
            MethodBuilder method = type.DefineMethod(methodName, methodAttributes, returnType, parameterTypes);
            method.SetImplementationFlags(methodImplAttributes);
            var generator = method.GetILGenerator();
            emitCode(generator);
        }

        /// <summary>
        /// <para>
        /// Emits the static method using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TDelegate">
        /// <para>The delegate.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodName">
        /// <para>The method name.</para>
        /// <para></para>
        /// </param>
        /// <param name="emitCode">
        /// <para>The emit code.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitStaticMethod<TDelegate>(this TypeBuilder type, string methodName, Action<ILGenerator> emitCode) => type.EmitMethod<TDelegate>(methodName, DefaultStaticMethodAttributes, DefaultMethodImplAttributes, emitCode);

        /// <summary>
        /// <para>
        /// Emits the final virtual method using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="TDelegate">
        /// <para>The delegate.</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="methodName">
        /// <para>The method name.</para>
        /// <para></para>
        /// </param>
        /// <param name="emitCode">
        /// <para>The emit code.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EmitFinalVirtualMethod<TDelegate>(this TypeBuilder type, string methodName, Action<ILGenerator> emitCode) => type.EmitMethod<TDelegate>(methodName, DefaultFinalVirtualMethodAttributes, DefaultMethodImplAttributes, emitCode);
    }
}
