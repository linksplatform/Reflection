using System;
using System.Collections.Generic;

namespace Platform.Reflection
{
    public class Types<T1, T2> : Types
    {
        public static readonly IList<Type> List = new Types<T1, T2>().ToReadOnlyList();
        private Types() { }
    }
}
