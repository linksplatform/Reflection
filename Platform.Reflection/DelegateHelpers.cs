using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Platform.Exceptions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public static class DelegateHelpers
    {
        public static TDelegate CompileOrDefault<TDelegate>(Action<ILGenerator> emitCode, bool aggressiveInlining)
            where TDelegate : Delegate
        {
            var @delegate = default(TDelegate);
            try
            {
                @delegate = aggressiveInlining ? CompileUsingMethodBuilder<TDelegate>(emitCode) : CompileUsingDynamicMethod<TDelegate>(emitCode);
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
            return @delegate;
        }

        public static TDelegate CompileOrDefault<TDelegate>(Action<ILGenerator> emitCode) where TDelegate : Delegate => CompileOrDefault<TDelegate>(emitCode, false);

        public static TDelegate Compile<TDelegate>(Action<ILGenerator> emitCode, bool aggressiveInlining)
            where TDelegate : Delegate
        {
            var @delegate = CompileOrDefault<TDelegate>(emitCode, aggressiveInlining);
            if (EqualityComparer<TDelegate>.Default.Equals(@delegate, default))
            {
                @delegate = new NotSupportedExceptionDelegateFactory<TDelegate>().Create();
            }
            return @delegate;
        }

        public static TDelegate Compile<TDelegate>(Action<ILGenerator> emitCode) where TDelegate : Delegate => Compile<TDelegate>(emitCode, false);

        private static TDelegate CompileUsingDynamicMethod<TDelegate>(Action<ILGenerator> emitCode)
        {
            var delegateType = typeof(TDelegate);
            var invoke = delegateType.GetMethod("Invoke");
            var returnType = invoke.ReturnType;
            var parameterTypes = invoke.GetParameters().Select(s => s.ParameterType).ToArray();
            var dynamicMethod = new DynamicMethod(GetNewName(), returnType, parameterTypes);
            var generator = dynamicMethod.GetILGenerator();
            emitCode(generator);
            return (TDelegate)(object)dynamicMethod.CreateDelegate(delegateType);
        }

        private static TDelegate CompileUsingMethodBuilder<TDelegate>(Action<ILGenerator> emitCode)
        {
            AssemblyName assemblyName = new AssemblyName(GetNewName());
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(GetNewName());
            var type = module.DefineType(GetNewName());
            var delegateType = typeof(TDelegate);
            var invoke = delegateType.GetMethod("Invoke");
            var returnType = invoke.ReturnType;
            var parameterTypes = invoke.GetParameters().Select(s => s.ParameterType).ToArray();
            var methodName = GetNewName();
            MethodBuilder method = type.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static, returnType, parameterTypes);
            method.SetImplementationFlags(MethodImplAttributes.IL | MethodImplAttributes.Managed | MethodImplAttributes.AggressiveInlining);
            var generator = method.GetILGenerator();
            emitCode(generator);
            var typeInfo = type.CreateTypeInfo();
            return (TDelegate)(object)typeInfo.GetMethod(methodName).CreateDelegate(delegateType);
        }

        private static string GetNewName() => Guid.NewGuid().ToString("N");
    }
}
