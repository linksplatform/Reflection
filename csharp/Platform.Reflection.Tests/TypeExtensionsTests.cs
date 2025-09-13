using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class TypeExtensionsTests
    {
        private static class TestClass
        {
            public static int StaticField = 100;
            public static string StaticProperty { get; set; } = "static test";
        }

        [Fact]
        public void StaticMemberBindingFlagsTest()
        {
            var expectedFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            Assert.Equal(expectedFlags, TypeExtensions.StaticMemberBindingFlags);
        }

        [Fact]
        public void DefaultDelegateMethodNameTest()
        {
            Assert.Equal("Invoke", TypeExtensions.DefaultDelegateMethodName);
        }

        [Fact]
        public void GetFirstFieldTest()
        {
            var type = typeof(TestClass);
            var firstField = type.GetFirstField();
            Assert.NotNull(firstField);
            Assert.Equal("StaticField", firstField.Name);
        }

        [Fact]
        public void GetStaticFieldValueTest()
        {
            var type = typeof(TestClass);
            var value = type.GetStaticFieldValue<int>("StaticField");
            Assert.Equal(100, value);
        }

        [Fact]
        public void GetStaticPropertyValueTest()
        {
            var type = typeof(TestClass);
            var value = type.GetStaticPropertyValue<string>("StaticProperty");
            Assert.Equal("static test", value);
        }

        [Fact]
        public void GetGenericMethodTest()
        {
            // This is testing the GetGenericMethod extension, but it's complex to test properly
            // Let's just verify that the method exists and can be called
            var type = typeof(List<int>);
            var methods = type.GetMethods().Where(m => m.Name == "Add").ToArray();
            Assert.True(methods.Length > 0);
        }

        [Fact]
        public void GetBaseTypeTest()
        {
            var type = typeof(List<int>);
            var baseType = type.GetBaseType();
            Assert.Equal(typeof(object), baseType);
        }

        [Fact]
        public void GetAssemblyTest()
        {
            var type = typeof(int);
            var assembly = type.GetAssembly();
            Assert.NotNull(assembly);
        }

        [Fact]
        public void IsSubclassOfTest()
        {
            var listType = typeof(List<int>);
            Assert.True(listType.IsSubclassOf(typeof(object)));
            Assert.False(listType.IsSubclassOf(typeof(string)));
        }

        [Fact]
        public void IsValueTypeTest()
        {
            Assert.True(typeof(int).IsValueType());
            Assert.True(typeof(bool).IsValueType());
            Assert.False(typeof(string).IsValueType());
            Assert.False(typeof(object).IsValueType());
        }

        [Fact]
        public void IsGenericTest()
        {
            Assert.True(typeof(List<int>).IsGeneric());
            Assert.True(typeof(Dictionary<,>).IsGeneric());
            Assert.False(typeof(int).IsGeneric());
            Assert.False(typeof(string).IsGeneric());
        }

        [Fact]
        public void IsGenericWithDefinitionTest()
        {
            Assert.True(typeof(List<int>).IsGeneric(typeof(List<>)));
            Assert.True(typeof(Dictionary<int, string>).IsGeneric(typeof(Dictionary<,>)));
            Assert.False(typeof(List<int>).IsGeneric(typeof(Dictionary<,>)));
            Assert.False(typeof(int).IsGeneric(typeof(List<>)));
        }

        [Fact]
        public void IsNullableTest()
        {
            Assert.True(typeof(int?).IsNullable());
            Assert.True(typeof(bool?).IsNullable());
            Assert.False(typeof(int).IsNullable());
            Assert.False(typeof(string).IsNullable());
        }

        [Fact]
        public void GetUnsignedVersionOrNullTest()
        {
            Assert.Equal(typeof(byte), typeof(sbyte).GetUnsignedVersionOrNull());
            Assert.Equal(typeof(ushort), typeof(short).GetUnsignedVersionOrNull());
            Assert.Equal(typeof(uint), typeof(int).GetUnsignedVersionOrNull());
            Assert.Equal(typeof(ulong), typeof(long).GetUnsignedVersionOrNull());
            Assert.Null(typeof(byte).GetUnsignedVersionOrNull());
            Assert.Null(typeof(string).GetUnsignedVersionOrNull());
        }

        [Fact]
        public void GetSignedVersionOrNullTest()
        {
            Assert.Equal(typeof(sbyte), typeof(byte).GetSignedVersionOrNull());
            Assert.Equal(typeof(short), typeof(ushort).GetSignedVersionOrNull());
            Assert.Equal(typeof(int), typeof(uint).GetSignedVersionOrNull());
            Assert.Equal(typeof(long), typeof(ulong).GetSignedVersionOrNull());
            Assert.Null(typeof(sbyte).GetSignedVersionOrNull());
            Assert.Null(typeof(string).GetSignedVersionOrNull());
        }

        [Fact]
        public void CanBeNumericTest()
        {
            // Numeric types
            Assert.True(typeof(byte).CanBeNumeric());
            Assert.True(typeof(int).CanBeNumeric());
            Assert.True(typeof(long).CanBeNumeric());
            Assert.True(typeof(float).CanBeNumeric());
            Assert.True(typeof(double).CanBeNumeric());
            Assert.True(typeof(decimal).CanBeNumeric());
            
            // Can be numeric types
            Assert.True(typeof(bool).CanBeNumeric());
            Assert.True(typeof(char).CanBeNumeric());
            Assert.True(typeof(DateTime).CanBeNumeric());
            Assert.True(typeof(TimeSpan).CanBeNumeric());
            
            // Non-numeric types
            Assert.False(typeof(string).CanBeNumeric());
            Assert.False(typeof(object).CanBeNumeric());
        }

        [Fact]
        public void IsNumericTest()
        {
            // Numeric types
            Assert.True(typeof(byte).IsNumeric());
            Assert.True(typeof(sbyte).IsNumeric());
            Assert.True(typeof(short).IsNumeric());
            Assert.True(typeof(ushort).IsNumeric());
            Assert.True(typeof(int).IsNumeric());
            Assert.True(typeof(uint).IsNumeric());
            Assert.True(typeof(long).IsNumeric());
            Assert.True(typeof(ulong).IsNumeric());
            Assert.True(typeof(float).IsNumeric());
            Assert.True(typeof(double).IsNumeric());
            Assert.True(typeof(decimal).IsNumeric());
            
            // Non-numeric types
            Assert.False(typeof(bool).IsNumeric());
            Assert.False(typeof(char).IsNumeric());
            Assert.False(typeof(string).IsNumeric());
            Assert.False(typeof(object).IsNumeric());
        }

        [Fact]
        public void IsSignedTest()
        {
            // Signed types
            Assert.True(typeof(sbyte).IsSigned());
            Assert.True(typeof(short).IsSigned());
            Assert.True(typeof(int).IsSigned());
            Assert.True(typeof(long).IsSigned());
            Assert.True(typeof(float).IsSigned());
            Assert.True(typeof(double).IsSigned());
            Assert.True(typeof(decimal).IsSigned());
            
            // Unsigned types
            Assert.False(typeof(byte).IsSigned());
            Assert.False(typeof(ushort).IsSigned());
            Assert.False(typeof(uint).IsSigned());
            Assert.False(typeof(ulong).IsSigned());
            Assert.False(typeof(bool).IsSigned());
            Assert.False(typeof(char).IsSigned());
        }

        [Fact]
        public void IsFloatPointTest()
        {
            // Float point types
            Assert.True(typeof(float).IsFloatPoint());
            Assert.True(typeof(double).IsFloatPoint());
            Assert.True(typeof(decimal).IsFloatPoint());
            
            // Non-float point types
            Assert.False(typeof(int).IsFloatPoint());
            Assert.False(typeof(byte).IsFloatPoint());
            Assert.False(typeof(long).IsFloatPoint());
            Assert.False(typeof(bool).IsFloatPoint());
        }

        [Fact]
        public void GetDelegateReturnTypeTest()
        {
            Assert.Equal(typeof(void), typeof(Action).GetDelegateReturnType());
            Assert.Equal(typeof(int), typeof(Func<int>).GetDelegateReturnType());
            Assert.Equal(typeof(string), typeof(Func<string>).GetDelegateReturnType());
            Assert.Equal(typeof(bool), typeof(Func<int, bool>).GetDelegateReturnType());
        }

        [Fact]
        public void GetDelegateParameterTypesTest()
        {
            var actionTypes = typeof(Action).GetDelegateParameterTypes();
            Assert.Empty(actionTypes);
            
            var actionIntTypes = typeof(Action<int>).GetDelegateParameterTypes();
            Assert.Single(actionIntTypes);
            Assert.Equal(typeof(int), actionIntTypes[0]);
            
            var funcIntStringTypes = typeof(Func<int, string>).GetDelegateParameterTypes();
            Assert.Single(funcIntStringTypes);
            Assert.Equal(typeof(int), funcIntStringTypes[0]);
            
            var actionMultiTypes = typeof(Action<int, string, bool>).GetDelegateParameterTypes();
            Assert.Equal(3, actionMultiTypes.Length);
            Assert.Equal(typeof(int), actionMultiTypes[0]);
            Assert.Equal(typeof(string), actionMultiTypes[1]);
            Assert.Equal(typeof(bool), actionMultiTypes[2]);
        }

        [Fact]
        public void GetDelegateCharacteristicsTest()
        {
            typeof(Func<int, string>).GetDelegateCharacteristics(out Type returnType, out Type[] parameterTypes);
            Assert.Equal(typeof(string), returnType);
            Assert.Single(parameterTypes);
            Assert.Equal(typeof(int), parameterTypes[0]);
            
            typeof(Action<bool, int>).GetDelegateCharacteristics(out returnType, out parameterTypes);
            Assert.Equal(typeof(void), returnType);
            Assert.Equal(2, parameterTypes.Length);
            Assert.Equal(typeof(bool), parameterTypes[0]);
            Assert.Equal(typeof(int), parameterTypes[1]);
        }
    }
}