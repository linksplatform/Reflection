using System;
using System.Reflection;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class MethodInfoExtensionsTests
    {
        private static class TestClass
        {
            public static void NoParametersMethod() { }
            public static void OneParameterMethod(int parameter) { }
            public static void TwoParametersMethod(int first, string second) { }
            public static void ManyParametersMethod(int a, string b, bool c, double d) { }
            
            public static int SimpleReturnMethod() => 42;
        }

        [Fact]
        public void GetParameterTypesNoParametersTest()
        {
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.NoParametersMethod));
            Assert.NotNull(methodInfo);
            
            var parameterTypes = methodInfo.GetParameterTypes();
            Assert.NotNull(parameterTypes);
            Assert.Empty(parameterTypes);
        }

        [Fact]
        public void GetParameterTypesOneParameterTest()
        {
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.OneParameterMethod));
            Assert.NotNull(methodInfo);
            
            var parameterTypes = methodInfo.GetParameterTypes();
            Assert.NotNull(parameterTypes);
            Assert.Single(parameterTypes);
            Assert.Equal(typeof(int), parameterTypes[0]);
        }

        [Fact]
        public void GetParameterTypesTwoParametersTest()
        {
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.TwoParametersMethod));
            Assert.NotNull(methodInfo);
            
            var parameterTypes = methodInfo.GetParameterTypes();
            Assert.NotNull(parameterTypes);
            Assert.Equal(2, parameterTypes.Length);
            Assert.Equal(typeof(int), parameterTypes[0]);
            Assert.Equal(typeof(string), parameterTypes[1]);
        }

        [Fact]
        public void GetParameterTypesManyParametersTest()
        {
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.ManyParametersMethod));
            Assert.NotNull(methodInfo);
            
            var parameterTypes = methodInfo.GetParameterTypes();
            Assert.NotNull(parameterTypes);
            Assert.Equal(4, parameterTypes.Length);
            Assert.Equal(typeof(int), parameterTypes[0]);
            Assert.Equal(typeof(string), parameterTypes[1]);
            Assert.Equal(typeof(bool), parameterTypes[2]);
            Assert.Equal(typeof(double), parameterTypes[3]);
        }

        [Fact]
        public void GetILBytesNotNullTest()
        {
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.SimpleReturnMethod));
            Assert.NotNull(methodInfo);
            
            var ilBytes = methodInfo.GetILBytes();
            Assert.NotNull(ilBytes);
            Assert.NotEmpty(ilBytes);
        }

        [Fact]
        public void GetILBytesForLambdaTest()
        {
            var func = new Func<int>(() => 42);
            var methodInfo = func.Method;
            
            var ilBytes = methodInfo.GetILBytes();
            Assert.NotNull(ilBytes);
            Assert.NotEmpty(ilBytes);
        }

        [Fact]
        public void GetParameterTypesForGenericMethodTest()
        {
            var method = typeof(Array).GetMethod("IndexOf", new[] { typeof(Array), typeof(object) });
            Assert.NotNull(method);
            
            var parameterTypes = method.GetParameterTypes();
            Assert.NotNull(parameterTypes);
            Assert.Equal(2, parameterTypes.Length);
            Assert.Equal(typeof(Array), parameterTypes[0]);
            Assert.Equal(typeof(object), parameterTypes[1]);
        }
    }
}