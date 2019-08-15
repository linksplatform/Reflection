using System;
using Platform.Collections.Lists;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public class Types<T> : Types
    {
        public static ReadOnlyCollection<Type> Collection { get; } = new Types<T>().ToReadOnlyCollection();
        public static Type[] Array => ((IList<Type>)Collection).ToArray();
        private Types() { }
    }
}
