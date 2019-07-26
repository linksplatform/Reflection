using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Platform.Collections;

namespace Platform.Reflection
{
    public static class TypeExtensions
    {
        static private readonly HashSet<Type> CanBeNumericTypes;
        static private readonly HashSet<Type> IsNumericTypes;
        static private readonly HashSet<Type> IsSignedTypes;
        static private readonly HashSet<Type> IsFloatPointTypes;
        static private readonly Dictionary<Type, Type> UnsignedVersionsOfSignedTypes;
        static private readonly Dictionary<Type, Type> SignedVersionsOfUnsignedTypes;

        static TypeExtensions()
        {
            CanBeNumericTypes = new HashSet<Type> { typeof(bool), typeof(char), typeof(DateTime), typeof(TimeSpan) };
            IsNumericTypes = new HashSet<Type> { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) };
            IsSignedTypes = new HashSet<Type> { typeof(sbyte), typeof(short), typeof(int), typeof(long) };
            IsFloatPointTypes = new HashSet<Type> { typeof(decimal), typeof(double), typeof(float) };
            UnsignedVersionsOfSignedTypes = new Dictionary<Type, Type>
            {
                { typeof(sbyte), typeof(byte) },
                { typeof(short), typeof(ushort) },
                { typeof(int), typeof(uint) },
                { typeof(long), typeof(ulong) },
            };
            SignedVersionsOfUnsignedTypes = new Dictionary<Type, Type>
            {
                { typeof(byte), typeof(sbyte)},
                { typeof(ushort), typeof(short) },
                { typeof(uint), typeof(int) },
                { typeof(ulong), typeof(long) },
            };
            CanBeNumericTypes.UnionWith(IsNumericTypes);
            CanBeNumericTypes.UnionWith(IsSignedTypes);
            IsNumericTypes.UnionWith(IsSignedTypes);
            CanBeNumericTypes.UnionWith(IsFloatPointTypes);
            IsNumericTypes.UnionWith(IsFloatPointTypes);
            IsSignedTypes.UnionWith(IsFloatPointTypes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetFirstField(this Type type) => type.GetFields()[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticFieldValue<T>(this Type type, string name) => type.GetTypeInfo().GetField(name).GetStaticValue<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] genericParameterTypes, Type[] argumentTypes)
        {
            var methods = from m in type.GetMethods()
                          where m.Name == name
                             && m.IsGenericMethodDefinition
                          let typeParams = m.GetGenericArguments()
                          let normalParams = m.GetParameters().Select(x => x.ParameterType)
                          where typeParams.SequenceEqual(genericParameterTypes)
                             && normalParams.SequenceEqual(argumentTypes)
                          select m;
            var method = methods.Single();
            return method;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetBaseType(this Type type) => type.GetTypeInfo().BaseType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Assembly GetAssembly(this Type type) => type.GetTypeInfo().Assembly;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubclassOf(this Type type, Type superClass) => type.GetTypeInfo().IsSubclassOf(superClass);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueType(this Type type) => type.GetTypeInfo().IsValueType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGeneric(this Type type) => type.GetTypeInfo().IsGenericType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGeneric(this Type type, Type genericTypeDefinition) => type.IsGeneric() && type.GetGenericTypeDefinition() == genericTypeDefinition;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullable(this Type type) => type.IsGeneric(typeof(Nullable<>));

        public static Type GetUnsignedVersionOrNull(this Type signedType) => UnsignedVersionsOfSignedTypes.GetOrDefault(signedType);

        public static Type GetSignedVersionOrNull(this Type unsignedType) => SignedVersionsOfUnsignedTypes.GetOrDefault(unsignedType);

        public static bool CanBeNumeric(this Type type) => CanBeNumericTypes.Contains(type);

        public static bool IsNumeric(this Type type) => IsNumericTypes.Contains(type);

        public static bool IsSigned(this Type type) => IsSignedTypes.Contains(type);

        public static bool IsFloatPoint(this Type type) => IsFloatPointTypes.Contains(type);
    }
}
