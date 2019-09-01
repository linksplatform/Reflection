using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Platform.Exceptions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public static class DelegateHelpers
    {
        public static TDelegate CompileOrDefault<TDelegate>(Action<ILGenerator> emitCode)
            where TDelegate : Delegate
        {
            var @delegate = default(TDelegate);
            try
            {
                var delegateType = typeof(TDelegate);
                var invoke = delegateType.GetMethod("Invoke");
                var returnType = invoke.ReturnType;
                var parameterTypes = invoke.GetParameters().Select(s => s.ParameterType).ToArray();
                var dynamicMethod = new DynamicMethod(Guid.NewGuid().ToString(), returnType, parameterTypes);
                var generator = dynamicMethod.GetILGenerator();
                emitCode(generator);
                @delegate = (TDelegate)dynamicMethod.CreateDelegate(delegateType);
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
            return @delegate;
        }

        public static TDelegate Compile<TDelegate>(Action<ILGenerator> emitCode)
            where TDelegate : Delegate
        {
            var @delegate = CompileOrDefault<TDelegate>(emitCode);
            if (EqualityComparer<TDelegate>.Default.Equals(@delegate, default))
            {
                @delegate = new NotSupportedExceptionDelegateFactory<TDelegate>().Create();
            }
            return @delegate;
        }
    }
}
