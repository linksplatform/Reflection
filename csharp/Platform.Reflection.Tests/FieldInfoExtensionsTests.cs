using System;
using System.Reflection;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class FieldInfoExtensionsTests
    {
        private static class TestClass
        {
            public static int StaticIntField = 42;
            public static string StaticStringField = "test field";
            public static bool StaticBoolField = true;
            public static readonly DateTime StaticReadOnlyField = new DateTime(2023, 1, 1);
        }

        [Fact]
        public void GetStaticValueIntFieldTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticIntField));
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<int>();
            Assert.Equal(42, value);
        }

        [Fact]
        public void GetStaticValueStringFieldTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticStringField));
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<string>();
            Assert.Equal("test field", value);
        }

        [Fact]
        public void GetStaticValueBoolFieldTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticBoolField));
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<bool>();
            Assert.True(value);
        }

        [Fact]
        public void GetStaticValueObjectFieldTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticStringField));
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<object>();
            Assert.Equal("test field", value);
        }

        [Fact]
        public void GetStaticValueReadOnlyFieldTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticReadOnlyField));
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<DateTime>();
            Assert.Equal(new DateTime(2023, 1, 1), value);
        }

        [Fact]
        public void GetStaticValueWithFieldModificationTest()
        {
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticIntField));
            Assert.NotNull(fieldInfo);
            
            // Change the field value
            TestClass.StaticIntField = 100;
            
            var value = fieldInfo.GetStaticValue<int>();
            Assert.Equal(100, value);
            
            // Reset for other tests
            TestClass.StaticIntField = 42;
        }

        [Fact]
        public void GetStaticValueFromSystemTypeTest()
        {
            var fieldInfo = typeof(int).GetField("MaxValue");
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<int>();
            Assert.Equal(int.MaxValue, value);
        }

        [Fact]
        public void GetStaticValueFromEnumTest()
        {
            var fieldInfo = typeof(DayOfWeek).GetField("Monday");
            Assert.NotNull(fieldInfo);
            
            var value = fieldInfo.GetStaticValue<DayOfWeek>();
            Assert.Equal(DayOfWeek.Monday, value);
        }
    }
}