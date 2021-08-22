using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    /// <summary>
    /// <para>
    /// Represents the type extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// <para>
        /// The static.
        /// </para>
        /// <para></para>
        /// </summary>
        static public readonly BindingFlags StaticMemberBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        /// <summary>
        /// <para>
        /// The default delegate method name.
        /// </para>
        /// <para></para>
        /// </summary>
        static public readonly string DefaultDelegateMethodName = "Invoke";

        /// <summary>
        /// <para>
        /// The can be numeric types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly HashSet<Type> _canBeNumericTypes;
        /// <summary>
        /// <para>
        /// The is numeric types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly HashSet<Type> _isNumericTypes;
        /// <summary>
        /// <para>
        /// The is signed types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly HashSet<Type> _isSignedTypes;
        /// <summary>
        /// <para>
        /// The is float point types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly HashSet<Type> _isFloatPointTypes;
        /// <summary>
        /// <para>
        /// The unsigned versions of signed types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly Dictionary<Type, Type> _unsignedVersionsOfSignedTypes;
        /// <summary>
        /// <para>
        /// The signed versions of unsigned types.
        /// </para>
        /// <para></para>
        /// </summary>
        static private readonly Dictionary<Type, Type> _signedVersionsOfUnsignedTypes;

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TypeExtensions"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TypeExtensions()
        {
            _canBeNumericTypes = new HashSet<Type> { typeof(bool), typeof(char), typeof(DateTime), typeof(TimeSpan) };
            _isNumericTypes = new HashSet<Type> { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) };
            _canBeNumericTypes.UnionWith(_isNumericTypes);
            _isSignedTypes = new HashSet<Type> { typeof(sbyte), typeof(short), typeof(int), typeof(long) };
            _canBeNumericTypes.UnionWith(_isSignedTypes);
            _isNumericTypes.UnionWith(_isSignedTypes);
            _isFloatPointTypes = new HashSet<Type> { typeof(decimal), typeof(double), typeof(float) };
            _canBeNumericTypes.UnionWith(_isFloatPointTypes);
            _isNumericTypes.UnionWith(_isFloatPointTypes);
            _isSignedTypes.UnionWith(_isFloatPointTypes);
            _unsignedVersionsOfSignedTypes = new Dictionary<Type, Type>
            {
                { typeof(sbyte), typeof(byte) },
                { typeof(short), typeof(ushort) },
                { typeof(int), typeof(uint) },
                { typeof(long), typeof(ulong) },
            };
            _signedVersionsOfUnsignedTypes = new Dictionary<Type, Type>
            {
                { typeof(byte), typeof(sbyte)},
                { typeof(ushort), typeof(short) },
                { typeof(uint), typeof(int) },
                { typeof(ulong), typeof(long) },
            };
        }

        /// <summary>
        /// <para>
        /// Gets the first field using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The field info</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo GetFirstField(this Type type) => type.GetFields()[0];

        /// <summary>
        /// <para>
        /// Gets the static field value using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="name">
        /// <para>The name.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticFieldValue<T>(this Type type, string name) => type.GetField(name, StaticMemberBindingFlags).GetStaticValue<T>();

        /// <summary>
        /// <para>
        /// Gets the static property value using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The .</para>
        /// <para></para>
        /// </typeparam>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="name">
        /// <para>The name.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticPropertyValue<T>(this Type type, string name) => type.GetProperty(name, StaticMemberBindingFlags).GetStaticValue<T>();

        /// <summary>
        /// <para>
        /// Gets the generic method using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="name">
        /// <para>The name.</para>
        /// <para></para>
        /// </param>
        /// <param name="genericParameterTypes">
        /// <para>The generic parameter types.</para>
        /// <para></para>
        /// </param>
        /// <param name="argumentTypes">
        /// <para>The argument types.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The method.</para>
        /// <para></para>
        /// </returns>
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

        /// <summary>
        /// <para>
        /// Gets the base type using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetBaseType(this Type type) => type.BaseType;

        /// <summary>
        /// <para>
        /// Gets the assembly using the specified type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The assembly</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Assembly GetAssembly(this Type type) => type.Assembly;

        /// <summary>
        /// <para>
        /// Determines whether is subclass of.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="superClass">
        /// <para>The super.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubclassOf(this Type type, Type superClass) => type.IsSubclassOf(superClass);

        /// <summary>
        /// <para>
        /// Determines whether is value type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueType(this Type type) => type.IsValueType;

        /// <summary>
        /// <para>
        /// Determines whether is generic.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGeneric(this Type type) => type.IsGenericType;

        /// <summary>
        /// <para>
        /// Determines whether is generic.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <param name="genericTypeDefinition">
        /// <para>The generic type definition.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGeneric(this Type type, Type genericTypeDefinition) => type.IsGeneric() && type.GetGenericTypeDefinition() == genericTypeDefinition;

        /// <summary>
        /// <para>
        /// Determines whether is nullable.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullable(this Type type) => type.IsGeneric(typeof(Nullable<>));

        /// <summary>
        /// <para>
        /// Gets the unsigned version or null using the specified signed type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="signedType">
        /// <para>The signed type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetUnsignedVersionOrNull(this Type signedType) => _unsignedVersionsOfSignedTypes.GetOrDefault(signedType);

        /// <summary>
        /// <para>
        /// Gets the signed version or null using the specified unsigned type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="unsignedType">
        /// <para>The unsigned type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetSignedVersionOrNull(this Type unsignedType) => _signedVersionsOfUnsignedTypes.GetOrDefault(unsignedType);

        /// <summary>
        /// <para>
        /// Determines whether can be numeric.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanBeNumeric(this Type type) => _canBeNumericTypes.Contains(type);

        /// <summary>
        /// <para>
        /// Determines whether is numeric.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumeric(this Type type) => _isNumericTypes.Contains(type);

        /// <summary>
        /// <para>
        /// Determines whether is signed.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSigned(this Type type) => _isSignedTypes.Contains(type);

        /// <summary>
        /// <para>
        /// Determines whether is float point.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="type">
        /// <para>The type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFloatPoint(this Type type) => _isFloatPointTypes.Contains(type);

        /// <summary>
        /// <para>
        /// Gets the delegate return type using the specified delegate type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="delegateType">
        /// <para>The delegate type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetDelegateReturnType(this Type delegateType) => delegateType.GetMethod(DefaultDelegateMethodName).ReturnType;

        /// <summary>
        /// <para>
        /// Gets the delegate parameter types using the specified delegate type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="delegateType">
        /// <para>The delegate type.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The type array</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetDelegateParameterTypes(this Type delegateType) => delegateType.GetMethod(DefaultDelegateMethodName).GetParameterTypes();

        /// <summary>
        /// <para>
        /// Gets the delegate characteristics using the specified delegate type.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="delegateType">
        /// <para>The delegate type.</para>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetDelegateCharacteristics(this Type delegateType, out Type returnType, out Type[] parameterTypes)
        {
            var invoke = delegateType.GetMethod(DefaultDelegateMethodName);
            returnType = invoke.ReturnType;
            parameterTypes = invoke.GetParameterTypes();
        }
    }
}
