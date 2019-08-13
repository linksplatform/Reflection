using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public class Types<T1, T2, T3, T4, T5, T6, T7> : Types
    {
        public static readonly IList<Type> List = new Types<T1, T2, T3, T4, T5, T6, T7>().ToReadOnlyList();
        private Types() { }
    }
}
