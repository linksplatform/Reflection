using System;
using System.Collections.ObjectModel;
using Xunit;

namespace Platform.Reflection.Tests
{
    public class TypesTests
    {
        [Fact]
        public void BaseTypesCollectionIsEmptyTest()
        {
            Assert.NotNull(Types.Collection);
            Assert.Empty(Types.Collection);
        }

        [Fact]
        public void BaseTypesArrayIsEmptyTest()
        {
            Assert.NotNull(Types.Array);
            Assert.Empty(Types.Array);
        }

        [Fact]
        public void SingleGenericTypeCollectionContainsOneTypeTest()
        {
            var collection = Types<int>.Collection;
            Assert.NotNull(collection);
            Assert.Single(collection);
            Assert.Contains(typeof(int), collection);
        }

        [Fact]
        public void SingleGenericTypeArrayContainsOneTypeTest()
        {
            var array = Types<int>.Array;
            Assert.NotNull(array);
            Assert.Single(array);
            Assert.Contains(typeof(int), array);
        }

        [Fact]
        public void TwoGenericTypesCollectionContainsBothTypesTest()
        {
            var collection = Types<int, string>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(2, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
        }

        [Fact]
        public void ThreeGenericTypesCollectionContainsAllTypesTest()
        {
            var collection = Types<int, string, bool>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(3, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
        }

        [Fact]
        public void FourGenericTypesCollectionContainsAllTypesTest()
        {
            var collection = Types<int, string, bool, double>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(4, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
            Assert.Contains(typeof(double), collection);
        }

        [Fact]
        public void FiveGenericTypesCollectionContainsAllTypesTest()
        {
            var collection = Types<int, string, bool, double, float>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(5, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
            Assert.Contains(typeof(double), collection);
            Assert.Contains(typeof(float), collection);
        }

        [Fact]
        public void SixGenericTypesCollectionContainsAllTypesTest()
        {
            var collection = Types<int, string, bool, double, float, decimal>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(6, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
            Assert.Contains(typeof(double), collection);
            Assert.Contains(typeof(float), collection);
            Assert.Contains(typeof(decimal), collection);
        }

        [Fact]
        public void SevenGenericTypesCollectionContainsAllTypesTest()
        {
            var collection = Types<int, string, bool, double, float, decimal, char>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(7, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
            Assert.Contains(typeof(double), collection);
            Assert.Contains(typeof(float), collection);
            Assert.Contains(typeof(decimal), collection);
            Assert.Contains(typeof(char), collection);
        }

        [Fact]
        public void NestedTypesCollectionFlattenedTest()
        {
            // This tests the recursive nature of ToReadOnlyCollection
            var collection = Types<Types<int, string>, bool>.Collection;
            Assert.NotNull(collection);
            Assert.Equal(3, collection.Count);
            Assert.Contains(typeof(int), collection);
            Assert.Contains(typeof(string), collection);
            Assert.Contains(typeof(bool), collection);
        }

        [Fact]
        public void CollectionIsReadOnlyTest()
        {
            var collection = Types<int>.Collection;
            Assert.IsType<ReadOnlyCollection<Type>>(collection);
        }

        [Fact]
        public void ArrayIsNewInstanceEachTimeTest()
        {
            var array1 = Types<int>.Array;
            var array2 = Types<int>.Array;
            Assert.NotSame(array1, array2);
            Assert.Equal(array1, array2);
        }
    }
}