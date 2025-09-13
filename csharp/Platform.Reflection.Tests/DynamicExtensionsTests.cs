using System;
using System.Collections.Generic;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class DynamicExtensionsTests
    {
        private class TestClass
        {
            public int IntProperty { get; set; }
            public string StringProperty { get; set; } = "test";
            public bool BoolProperty { get; set; }
        }

        [Fact]
        public void HasPropertyExistingPropertyTest()
        {
            var obj = new TestClass();
            Assert.True(obj.HasProperty("IntProperty"));
            Assert.True(obj.HasProperty("StringProperty"));
            Assert.True(obj.HasProperty("BoolProperty"));
        }

        [Fact]
        public void HasPropertyNonExistingPropertyTest()
        {
            var obj = new TestClass();
            Assert.False(obj.HasProperty("NonExistentProperty"));
            Assert.False(obj.HasProperty("AnotherMissingProperty"));
        }

        [Fact]
        public void HasPropertyCaseSensitiveTest()
        {
            var obj = new TestClass();
            Assert.True(obj.HasProperty("IntProperty"));
            Assert.False(obj.HasProperty("intproperty"));
            Assert.False(obj.HasProperty("INTPROPERTY"));
            Assert.False(obj.HasProperty("intProperty"));
        }

        [Fact]
        public void HasPropertyWithDictionaryTest()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Key1", "Value1" },
                { "Key2", 42 },
                { "Key3", true }
            };

            // The current implementation doesn't actually check if the object is a dictionary
            // It checks if the type "is" IDictionary<string, object> which fails for Dictionary<string, object>
            // So it will fall back to checking properties on the Dictionary type itself
            object obj = dictionary;
            
            // Dictionary<string, object> has properties like Keys, Values, Count, etc.
            Assert.True(obj.HasProperty("Keys"));
            Assert.True(obj.HasProperty("Values"));
            Assert.True(obj.HasProperty("Count"));
            Assert.False(obj.HasProperty("Key1")); // These are dictionary entries, not properties
        }

        [Fact]
        public void HasPropertyWithEmptyDictionaryTest()
        {
            var dictionary = new Dictionary<string, object>();
            object obj = dictionary;
            // Same as above - it checks Dictionary type properties, not dictionary contents
            Assert.True(obj.HasProperty("Count")); // Dictionary has Count property
            Assert.False(obj.HasProperty("AnyKey")); // But not custom keys
        }

        [Fact]
        public void HasPropertyWithNullPropertyNameTest()
        {
            var obj = new TestClass();
            // The current implementation throws ArgumentNullException when propertyName is null
            // because Type.GetProperty(null) throws
            Assert.Throws<ArgumentNullException>(() => obj.HasProperty(null));
        }

        [Fact]
        public void HasPropertyWithEmptyPropertyNameTest()
        {
            var obj = new TestClass();
            Assert.False(obj.HasProperty(""));
            Assert.False(obj.HasProperty(string.Empty));
        }

        [Fact]
        public void HasPropertyWithSystemObjectTest()
        {
            var obj = new object();
            // Object class has standard properties like GetType, ToString, etc.
            Assert.False(obj.HasProperty("GetType")); // GetType is a method, not a property
            Assert.False(obj.HasProperty("SomeProperty"));
        }

        [Fact]
        public void HasPropertyWithBuiltInTypesTest()
        {
            var str = "test string";
            object obj = str;
            Assert.True(obj.HasProperty("Length")); // String has Length property
            Assert.False(obj.HasProperty("Size"));

            var array = new int[] { 1, 2, 3 };
            obj = array;
            Assert.True(obj.HasProperty("Length")); // Array has Length property
            Assert.False(obj.HasProperty("Count"));
        }

        [Fact]
        public void HasPropertyWithAnonymousObjectTest()
        {
            var obj = new { Name = "Test", Age = 25, IsActive = true };
            Assert.True(obj.HasProperty("Name"));
            Assert.True(obj.HasProperty("Age"));
            Assert.True(obj.HasProperty("IsActive"));
            Assert.False(obj.HasProperty("Height"));
        }
    }
}