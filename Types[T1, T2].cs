using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public class Types<T1, T2> : Types
    {
        public static readonly IList<Type> List = new Types<T1, T2>().ToReadOnlyList();
        private Types() { }
    }
}
