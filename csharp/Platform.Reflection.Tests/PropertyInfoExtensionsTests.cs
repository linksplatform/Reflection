using System;
using System.Reflection;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class PropertyInfoExtensionsTests
    {
        private static class TestClass
        {
            public static int IntProperty { get; set; } = 42;
            public static string StringProperty { get; set; } = "test";
            public static bool BoolProperty { get; set; } = true;
        }

        [Fact]
        public void GetStaticValueIntPropertyTest()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.IntProperty));
            Assert.NotNull(propertyInfo);
            
            var value = propertyInfo.GetStaticValue<int>();
            Assert.Equal(42, value);
        }

        [Fact]
        public void GetStaticValueStringPropertyTest()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.StringProperty));
            Assert.NotNull(propertyInfo);
            
            var value = propertyInfo.GetStaticValue<string>();
            Assert.Equal("test", value);
        }

        [Fact]
        public void GetStaticValueBoolPropertyTest()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.BoolProperty));
            Assert.NotNull(propertyInfo);
            
            var value = propertyInfo.GetStaticValue<bool>();
            Assert.True(value);
        }

        [Fact]
        public void GetStaticValueObjectPropertyTest()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.StringProperty));
            Assert.NotNull(propertyInfo);
            
            var value = propertyInfo.GetStaticValue<object>();
            Assert.Equal("test", value);
        }

        [Fact]
        public void GetStaticValueWithChangedPropertyTest()
        {
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.IntProperty));
            Assert.NotNull(propertyInfo);
            
            TestClass.IntProperty = 100;
            var value = propertyInfo.GetStaticValue<int>();
            Assert.Equal(100, value);
            
            // Reset for other tests
            TestClass.IntProperty = 42;
        }
    }
}